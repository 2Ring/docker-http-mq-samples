# You need Docker desktop to execute this commands
# 

# build our images
docker-compose build

# deploy RabbitMQ
docker-compose -f docker-compose-dependencies.yaml up -d

# Wait about 10 seconds

# deploy our services
docker-compose up -d

