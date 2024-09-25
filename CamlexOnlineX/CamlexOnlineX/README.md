# Building docker image:
docker build -t {containers_registry_tag:x.x.x} .

# Pushing to the registry:
docker push {containers_registry_tag:x.x.x}