# robot cleaner
**Tibber Platform environment**
The Tibber platform consists of a swarm of microservices running as
Docker containers. The primary development platforms are .NET,
Node JS and Python in conjunction with other technologies. Our
main ways of data storage are through PostgreSQL as
relational/document storage and Amazon S3 as blob storage.

## Technical Case
Create a new microservice that could fit into the Tibber Platform environment as
described above. The created service will simulate a robot moving in an office space
and will be cleaning the places this robot visits. The path of the robot's movement is
described by the starting coordinates and move commands. After the cleaning has
been done, the robot reports the number of unique places cleaned. The service will
store the results into the database and return the created record in JSON format. The
service listens to HTTP protocol on port 5000.

## Notes
● You can assume, for the sake of simplicity, that the office can be viewed as a
grid where the robot moves only on the vertices.
● The robot cleans at every vertex it touches, not just where it stops.
● All input should be considered well formed and syntactically correct. There is no
need, therefore, to implement elaborate input validation.
● The robot will never be sent outside the bounds of the office.
● Ensure that database connection is configurable using environment variable.
● Think about structure, readability, maintainability, performance, re-usability and
test-ability of the code. Like the solution is going to be deployed into the
production environment. You should be proud of what you deliver.
● Use only open source dependencies if needed.
● Include Dockerfile and docker-compose configuration files in the solution.

## What will we be assessing?
Doing a technical case can be tricky and time intensive, we know! And you better
believe it, we are grateful for the time, effort and attention to detail you put into this! To
make it a bit clearer for you, we want to share some of the things that we will assess
when reviewing your assignment, please see below.
● WE LOVE unit testing! ♥️
● Troubleshooting is time consuming but please ensure your solution is runnable
on a clean machine using just the code and documentation you have provided
(ie. sometimes it can run well on a mac but not on other machines) 🏃
● We’re not huge fans of over engineered solutions. Crisp, clear and efficient is our
way to go, don’t overcomplicate something which doesn’t need to be. Clean code
is happy code.
