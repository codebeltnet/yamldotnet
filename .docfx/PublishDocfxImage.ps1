$version = minver -i -t v -v w
docker tag yamldotnet-docfx:$version jcr.codebelt.net/geekle/yamldotnet-docfx:$version
docker push jcr.codebelt.net/geekle/yamldotnet-docfx:$version
