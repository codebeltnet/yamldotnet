$version = minver -i -t v -v w
docker tag yamldotnet-docfx:$version jcr.codebelt.net/geekle/yamldotnet:$version
docker push yourbranding/classlibrary1:$version
