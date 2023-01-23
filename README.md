# Battleships game

## How to run a game

### Using dotnet

1) Make sure you have installed the `.NET SDK` for `.NET 7.0` on your machine
2) Change working directory using `cd src/Battleships.Console`
3) Run `dotnet run`
4) Play a game

### Using docker
1) Make sure you have installed docker on your machine
2) From root directory run `docker build -t battleships-game -f ./src/Battleships.Console/Dockerfile .` to build image
3) From root directory run `docker run -it battleships-game` to run container in interactive mode
4) Play a game

### Using docker-compose
1) Make sure you have installed docker on your machine
2) From root directory run `docker-compose -f ./docker-compose.yml run battleships -it` 
3) Play a game