# APIJobWithHangFire

To Test the APIJobWithHangFire you need can run the redis instance in docker

Run Redis Intance from Local  ------
Command1: pull latest instance from docker
docker pull redis:latest

Command2: run the redis instance named my-redis in detached  moe ("-d" indicates that)
docker run -d --name my-redis -p 6379:6379 redis:latest

----------------------------------------------------------------

Check  the hangfore dashboard -

https://localhost:{port}/hangfire


----------------------------------------------------------------

Verify the Data in Redis: 

Step1: Run in terminal - "docker exec -it my-redis redis-cli"

Step2: Step 1 should show a server terminal, then type- 
"keys *hangfire*" and enter.
