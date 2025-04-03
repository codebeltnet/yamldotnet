using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Codebelt.Extensions.AspNetCore.Text.Yaml.Formatters;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Xunit;
using Xunit.Abstractions;

namespace Codebelt.Extensions.AspNetCore.Text.Yaml
{
    public class ServiceCollectionExtensions : Test
    {
        public ServiceCollectionExtensions(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData(FaultSensitivityDetails.All)]
        [InlineData(FaultSensitivityDetails.Evidence)]
        [InlineData(FaultSensitivityDetails.FailureWithStackTraceAndData)]
        [InlineData(FaultSensitivityDetails.FailureWithData)]
        [InlineData(FaultSensitivityDetails.FailureWithStackTrace)]
        [InlineData(FaultSensitivityDetails.Failure)]
        [InlineData(FaultSensitivityDetails.None)]
        public async Task AddYamlExceptionResponseFormatter_ShouldCaptureException_RenderAsExceptionDescriptor_UsingYaml(FaultSensitivityDetails sensitivity)
        {
            using var response = await WebHostTestFactory.RunAsync(
                services =>
                {
                    services.AddFaultDescriptorOptions(o => o.FaultDescriptor = PreferredFaultDescriptor.FaultDetails);
                    services.AddYamlExceptionResponseFormatter();
                    services.PostConfigureAllOf<IExceptionDescriptorOptions>(o => o.SensitivityDetails = sensitivity);
                },
                app =>
                {
                    app.UseFaultDescriptorExceptionHandler();
                    app.Use(async (context, next) =>
                    {
                        try
                        {
                            throw new ArgumentException("This is an inner exception message ...", nameof(app))
                            {
                                Data =
                                {
                                    { "1st", "data value" }
                                },
                                HelpLink = "https://www.savvyio.net/"
                            };
                        }
                        catch (Exception e)
                        {
                            throw new NotFoundException("Main exception - look out for inner!", e);
                        }

                        await next(context);
                    });
                },
                responseFactory: client =>
                {
                    client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/yaml"));
                    return client.GetAsync("/");
                }, hostFixture: null);

            var body = await response.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            switch (sensitivity)
            {
                case FaultSensitivityDetails.All:
                    Assert.True(Match("""
                                      error:
                                        instance: http://localhost/
                                        status: 404
                                        code: NotFound
                                        message: Main exception - look out for inner!
                                        failure:
                                          type: Cuemon.AspNetCore.Http.NotFoundException
                                          source: Codebelt.Extensions.AspNetCore.Text.Yaml.Tests
                                          message: Main exception - look out for inner!
                                          stack:
                                            - at Codebelt.Extensions.AspNetCore.Text.Yaml.ServiceCollectionExtensions.<>c.<<AddYamlExceptionResponseFormatter_ShouldCaptureException_RenderAsExceptionDescriptor_UsingYaml*
                                            - '--- End of stack trace from previous location ---'
                                            - at Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddlewareImpl.<Invoke>g__Awaited|10_0(ExceptionHandlerMiddlewareImpl middleware, HttpContext context, Task task)
                                          statusCode: 404
                                          reasonPhrase: Not Found
                                          inner:
                                            type: System.ArgumentException
                                            source: Codebelt.Extensions.AspNetCore.Text.Yaml.Tests
                                            message: This is an inner exception message ... (Parameter 'app')
                                            stack:
                                              - at Codebelt.Extensions.AspNetCore.Text.Yaml.ServiceCollectionExtensions.<>c.<<AddYamlExceptionResponseFormatter_ShouldCaptureException_RenderAsExceptionDescriptor_UsingYaml*
                                            data:
                                              1st: data value
                                            paramName: app
                                      evidence:
                                        request:
                                          location: http://localhost/
                                          method: GET
                                          headers:
                                            Accept:
                                              - application/yaml
                                            Host:
                                              - localhost
                                          query: []
                                          cookies: []
                                          body: ''
                                      traceId: *
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
                case FaultSensitivityDetails.Evidence:
                    Assert.True(Match("""
                                      error:
                                        instance: http://localhost/
                                        status: 404
                                        code: NotFound
                                        message: Main exception - look out for inner!
                                      evidence:
                                        request:
                                          location: http://localhost/
                                          method: GET
                                          headers:
                                            Accept:
                                              - application/yaml
                                            Host:
                                              - localhost
                                          query: []
                                          cookies: []
                                          body: ''
                                      traceId: *
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
                case FaultSensitivityDetails.FailureWithStackTraceAndData:
                    Assert.True(Match("""
                                      error:
                                        instance: http://localhost/
                                        status: 404
                                        code: NotFound
                                        message: Main exception - look out for inner!
                                        failure:
                                          type: Cuemon.AspNetCore.Http.NotFoundException
                                          source: Codebelt.Extensions.AspNetCore.Text.Yaml.Tests
                                          message: Main exception - look out for inner!
                                          stack:
                                            - at Codebelt.Extensions.AspNetCore.Text.Yaml.ServiceCollectionExtensions.<>c.<<AddYamlExceptionResponseFormatter_ShouldCaptureException_RenderAsExceptionDescriptor_UsingYaml*
                                            - '--- End of stack trace from previous location ---'
                                            - at Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddlewareImpl.<Invoke>g__Awaited|10_0(ExceptionHandlerMiddlewareImpl middleware, HttpContext context, Task task)
                                          statusCode: 404
                                          reasonPhrase: Not Found
                                          inner:
                                            type: System.ArgumentException
                                            source: Codebelt.Extensions.AspNetCore.Text.Yaml.Tests
                                            message: This is an inner exception message ... (Parameter 'app')
                                            stack:
                                              - at Codebelt.Extensions.AspNetCore.Text.Yaml.ServiceCollectionExtensions.<>c.<<AddYamlExceptionResponseFormatter_ShouldCaptureException_RenderAsExceptionDescriptor_UsingYaml*
                                            data:
                                              1st: data value
                                            paramName: app
                                      traceId: *
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
                case FaultSensitivityDetails.FailureWithData:
                    Assert.True(Match("""
                                      error:
                                        instance: http://localhost/
                                        status: 404
                                        code: NotFound
                                        message: Main exception - look out for inner!
                                        failure:
                                          type: Cuemon.AspNetCore.Http.NotFoundException
                                          source: Codebelt.Extensions.AspNetCore.Text.Yaml.Tests
                                          message: Main exception - look out for inner!
                                          statusCode: 404
                                          reasonPhrase: Not Found
                                          inner:
                                            type: System.ArgumentException
                                            source: Codebelt.Extensions.AspNetCore.Text.Yaml.Tests
                                            message: This is an inner exception message ... (Parameter 'app')
                                            data:
                                              1st: data value
                                            paramName: app
                                      traceId: *
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
                case FaultSensitivityDetails.FailureWithStackTrace:
                    Assert.True(Match("""
                                      error:
                                        instance: http://localhost/
                                        status: 404
                                        code: NotFound
                                        message: Main exception - look out for inner!
                                        failure:
                                          type: Cuemon.AspNetCore.Http.NotFoundException
                                          source: Codebelt.Extensions.AspNetCore.Text.Yaml.Tests
                                          message: Main exception - look out for inner!
                                          stack:
                                            - at Codebelt.Extensions.AspNetCore.Text.Yaml.ServiceCollectionExtensions.<>c.<<AddYamlExceptionResponseFormatter_ShouldCaptureException_RenderAsExceptionDescriptor_UsingYaml*
                                            - '--- End of stack trace from previous location ---'
                                            - at Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddlewareImpl.<Invoke>g__Awaited|10_0(ExceptionHandlerMiddlewareImpl middleware, HttpContext context, Task task)
                                          statusCode: 404
                                          reasonPhrase: Not Found
                                          inner:
                                            type: System.ArgumentException
                                            source: Codebelt.Extensions.AspNetCore.Text.Yaml.Tests
                                            message: This is an inner exception message ... (Parameter 'app')
                                            stack:
                                              - at Codebelt.Extensions.AspNetCore.Text.Yaml.ServiceCollectionExtensions.<>c.<<AddYamlExceptionResponseFormatter_ShouldCaptureException_RenderAsExceptionDescriptor_UsingYaml*
                                            paramName: app
                                      traceId: *
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
                case FaultSensitivityDetails.Failure:
                    Assert.True(Match("""
                                      error:
                                        instance: http://localhost/
                                        status: 404
                                        code: NotFound
                                        message: Main exception - look out for inner!
                                        failure:
                                          type: Cuemon.AspNetCore.Http.NotFoundException
                                          source: Codebelt.Extensions.AspNetCore.Text.Yaml.Tests
                                          message: Main exception - look out for inner!
                                          statusCode: 404
                                          reasonPhrase: Not Found
                                          inner:
                                            type: System.ArgumentException
                                            source: Codebelt.Extensions.AspNetCore.Text.Yaml.Tests
                                            message: This is an inner exception message ... (Parameter 'app')
                                            paramName: app
                                      traceId: *
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
                case FaultSensitivityDetails.None:
                    Assert.True(Match("""
                                      error:
                                        instance: http://localhost/
                                        status: 404
                                        code: NotFound
                                        message: Main exception - look out for inner!
                                      traceId: *
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
            }
        }

        [Theory]
        [InlineData(FaultSensitivityDetails.All)]
        [InlineData(FaultSensitivityDetails.Evidence)]
        [InlineData(FaultSensitivityDetails.FailureWithStackTraceAndData)]
        [InlineData(FaultSensitivityDetails.FailureWithData)]
        [InlineData(FaultSensitivityDetails.FailureWithStackTrace)]
        [InlineData(FaultSensitivityDetails.Failure)]
        [InlineData(FaultSensitivityDetails.None)]
        public async Task AddYamlExceptionResponseFormatter_ShouldCaptureException_RenderAsProblemDetails_UsingYaml(FaultSensitivityDetails sensitivity)
        {
            using var response = await WebHostTestFactory.RunAsync(
                services =>
                {
                    services.AddFaultDescriptorOptions(o => o.FaultDescriptor = PreferredFaultDescriptor.ProblemDetails);
                    services.AddYamlExceptionResponseFormatter();
                    services.PostConfigureAllOf<IExceptionDescriptorOptions>(o => o.SensitivityDetails = sensitivity);
                },
                app =>
                {
                    app.UseFaultDescriptorExceptionHandler();
                    app.Use(async (context, next) =>
                    {
                        try
                        {
                            throw new ArgumentException("This is an inner exception message ...", nameof(app))
                            {
                                Data =
                                {
                                    { "1st", "data value" }
                                },
                                HelpLink = "https://www.savvyio.net/"
                            };
                        }
                        catch (Exception e)
                        {
                            throw new NotFoundException("Main exception - look out for inner!", e);
                        }

                        await next(context);
                    });
                },
                responseFactory: client =>
                {
                    client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/yaml"));
                    return client.GetAsync("/");
                }, hostFixture: null);

            var body = await response.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            switch (sensitivity)
            {
                case FaultSensitivityDetails.All:
                    Assert.True(Match("""
                                      type: about:blank
                                      title: NotFound
                                      status: 404
                                      detail: Main exception - look out for inner!
                                      instance: http://localhost/
                                      traceId: *
                                      failure:
                                        type: Cuemon.AspNetCore.Http.NotFoundException
                                        source: Codebelt.Extensions.AspNetCore.Text.Yaml.Tests
                                        message: Main exception - look out for inner!
                                        stack:
                                          - at Codebelt.Extensions.AspNetCore.Text.Yaml.ServiceCollectionExtensions.<>c.<<AddYamlExceptionResponseFormatter_ShouldCaptureException_RenderAsProblemDetails_UsingYaml*
                                          - '--- End of stack trace from previous location ---'
                                          - at Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddlewareImpl.<Invoke>g__Awaited|10_0(ExceptionHandlerMiddlewareImpl middleware, HttpContext context, Task task)
                                        statusCode: 404
                                        reasonPhrase: Not Found
                                        inner:
                                          type: System.ArgumentException
                                          source: Codebelt.Extensions.AspNetCore.Text.Yaml.Tests
                                          message: This is an inner exception message ... (Parameter 'app')
                                          stack:
                                            - at Codebelt.Extensions.AspNetCore.Text.Yaml.ServiceCollectionExtensions.<>c.<<AddYamlExceptionResponseFormatter_ShouldCaptureException_RenderAsProblemDetails_UsingYaml*
                                          data:
                                            1st: data value
                                          paramName: app
                                      request:
                                        location: http://localhost/
                                        method: GET
                                        headers:
                                          Accept:
                                            - application/yaml
                                          Host:
                                            - localhost
                                        query: []
                                        cookies: []
                                        body: ''
                                      
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
                case FaultSensitivityDetails.Evidence:
                    Assert.True(Match("""
                                      type: about:blank
                                      title: NotFound
                                      status: 404
                                      detail: Main exception - look out for inner!
                                      instance: http://localhost/
                                      traceId: *
                                      request:
                                        location: http://localhost/
                                        method: GET
                                        headers:
                                          Accept:
                                            - application/yaml
                                          Host:
                                            - localhost
                                        query: []
                                        cookies: []
                                        body: ''
                                      
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
                case FaultSensitivityDetails.FailureWithStackTraceAndData:
                    Assert.True(Match("""
                                      type: about:blank
                                      title: NotFound
                                      status: 404
                                      detail: Main exception - look out for inner!
                                      instance: http://localhost/
                                      traceId: *
                                      failure:
                                        type: Cuemon.AspNetCore.Http.NotFoundException
                                        source: Codebelt.Extensions.AspNetCore.Text.Yaml.Tests
                                        message: Main exception - look out for inner!
                                        stack:
                                          - at Codebelt.Extensions.AspNetCore.Text.Yaml.ServiceCollectionExtensions.<>c.<<AddYamlExceptionResponseFormatter_ShouldCaptureException_RenderAsProblemDetails_UsingYaml*
                                          - '--- End of stack trace from previous location ---'
                                          - at Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddlewareImpl.<Invoke>g__Awaited|10_0(ExceptionHandlerMiddlewareImpl middleware, HttpContext context, Task task)
                                        statusCode: 404
                                        reasonPhrase: Not Found
                                        inner:
                                          type: System.ArgumentException
                                          source: Codebelt.Extensions.AspNetCore.Text.Yaml.Tests
                                          message: This is an inner exception message ... (Parameter 'app')
                                          stack:
                                            - at Codebelt.Extensions.AspNetCore.Text.Yaml.ServiceCollectionExtensions.<>c.<<AddYamlExceptionResponseFormatter_ShouldCaptureException_RenderAsProblemDetails_UsingYaml*
                                          data:
                                            1st: data value
                                          paramName: app
                                      
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
                case FaultSensitivityDetails.FailureWithData:
                    Assert.True(Match("""
                                      type: about:blank
                                      title: NotFound
                                      status: 404
                                      detail: Main exception - look out for inner!
                                      instance: http://localhost/
                                      traceId: *
                                      failure:
                                        type: Cuemon.AspNetCore.Http.NotFoundException
                                        source: Codebelt.Extensions.AspNetCore.Text.Yaml.Tests
                                        message: Main exception - look out for inner!
                                        statusCode: 404
                                        reasonPhrase: Not Found
                                        inner:
                                          type: System.ArgumentException
                                          source: Codebelt.Extensions.AspNetCore.Text.Yaml.Tests
                                          message: This is an inner exception message ... (Parameter 'app')
                                          data:
                                            1st: data value
                                          paramName: app
                                      
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
                case FaultSensitivityDetails.FailureWithStackTrace:
                    Assert.True(Match("""
                                      type: about:blank
                                      title: NotFound
                                      status: 404
                                      detail: Main exception - look out for inner!
                                      instance: http://localhost/
                                      traceId: *
                                      failure:
                                        type: Cuemon.AspNetCore.Http.NotFoundException
                                        source: Codebelt.Extensions.AspNetCore.Text.Yaml.Tests
                                        message: Main exception - look out for inner!
                                        stack:
                                          - at Codebelt.Extensions.AspNetCore.Text.Yaml.ServiceCollectionExtensions.<>c.<<AddYamlExceptionResponseFormatter_ShouldCaptureException_RenderAsProblemDetails_UsingYaml*
                                          - '--- End of stack trace from previous location ---'
                                          - at Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddlewareImpl.<Invoke>g__Awaited|10_0(ExceptionHandlerMiddlewareImpl middleware, HttpContext context, Task task)
                                        statusCode: 404
                                        reasonPhrase: Not Found
                                        inner:
                                          type: System.ArgumentException
                                          source: Codebelt.Extensions.AspNetCore.Text.Yaml.Tests
                                          message: This is an inner exception message ... (Parameter 'app')
                                          stack:
                                            - at Codebelt.Extensions.AspNetCore.Text.Yaml.ServiceCollectionExtensions.<>c.<<AddYamlExceptionResponseFormatter_ShouldCaptureException_RenderAsProblemDetails_UsingYaml*
                                          paramName: app
                                      
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
                case FaultSensitivityDetails.Failure:
                    Assert.True(Match("""
                                      type: about:blank
                                      title: NotFound
                                      status: 404
                                      detail: Main exception - look out for inner!
                                      instance: http://localhost/
                                      traceId: *
                                      failure:
                                        type: Cuemon.AspNetCore.Http.NotFoundException
                                        source: Codebelt.Extensions.AspNetCore.Text.Yaml.Tests
                                        message: Main exception - look out for inner!
                                        statusCode: 404
                                        reasonPhrase: Not Found
                                        inner:
                                          type: System.ArgumentException
                                          source: Codebelt.Extensions.AspNetCore.Text.Yaml.Tests
                                          message: This is an inner exception message ... (Parameter 'app')
                                          paramName: app
                                      
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
                case FaultSensitivityDetails.None:
                    Assert.True(Match("""
                                      type: about:blank
                                      title: NotFound
                                      status: 404
                                      detail: Main exception - look out for inner!
                                      instance: http://localhost/
                                      traceId: *
                                      
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
            }
        }
    }
}
