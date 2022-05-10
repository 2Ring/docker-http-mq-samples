# You need Docker desktop to execute this commands
# 

docker-compose -f docker-compose-build.yaml build

docker-compose -f docker-compose-dependencies.yaml up -d

# Wait about 10 seconds

docker-compose up -d

