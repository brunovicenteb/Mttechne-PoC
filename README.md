#  Prova conceitual de arquitetura limpa para Microsserviço

This is a proof of concept of an MVC application using clean architecture using .Net Core 7 with Razor Pages.

#[![Build-Tests](https://github.com/brunovicenteb/Mttechne-PoC/actions/workflows/Build-Test-Coverage.yml/badge.svg?branch=main)](https://github.com/brunovicenteb/Mttechne-PoC/actions/workflows/Build-Test-Coverage.yml)

<table>
  <tr>
    <th></th>
    <th>Technology</th>
    <th>Version</th>
    <th>Tools</th>    
  </tr>
  <tr>
    <td><img align="center" alt="Rafa-Csharp" height="30" width="40" src="https://icongr.am/devicon/dot-net-original.svg?size=40"></td>
    <td>.Net Core</td>
    <td>7.0</td>
    <td><a href="https://xunit.net/">XUnit</a></td>
  </tr>
  <tr>
    <td><img align="center" alt="Rafa-Csharp" height="30" width="40" src="https://icongr.am/devicon/csharp-original.svg?size=40"></td>
    <td>C#</td>
    <td>10.0</td>
    <td></td>
  </tr>    
  <tr>
    <td><img align="center" alt="Rafa-Csharp" height="30" width="40" src="https://icongr.am/devicon/visualstudio-plain.svg?size=40"></td>
    <td>Visual Studio</td>
    <td>2022 Community</td>
    <td></td>
  </tr>    
  <tr>
    <td><img align="center" alt="Rafa-Csharp" height="30" width="40" src="https://icongr.am/devicon/postgresql-original.svg?size=40"></td>
    <td><a href="https://www.postgresql.org/">Postgres</a></td>
    <td>lasted</td>
    <td></td>    
  </tr> 
  <tr>
    <td><img align="center" alt="Rafa-Csharp" height="30" width="40" src="https://icongr.am/devicon/docker-original.svg?size=40"></td>
    <td><a href="https://www.docker.com/">Docker</a></td>
    <td>lasted</td>
    <td><a href="https://docs.docker.com/compose">Docker Compose</a></td>    
  </tr>
</table>

## Prerequisites for installing the project

+ Git
+ Docker
+ Docker-compose

## Project installation and environment configuration:

1. Clone the repository

   `
   git clone https://github.com/brunovicenteb/Mttechne-PoC.git
   `

2. Open a command line on created directory:

   `
   cd Mttechne-PoC\src
   `

4. Run docker compose

   `
   docker compose up
   `
5. Access the application

   `
   http://localhost:8001/
   `