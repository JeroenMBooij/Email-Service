# EmailService

This repository contains the source code for a simple email service API running in Docker <br>

<h1> Usage </h1>
  * prerequisite - docker installed
  
  1. run docker-compose up
  2. open localhost:2000
  
 <h1> Development </h1>
  To use the Email service with other containers you have to create a docker network named <code>my-proxy-net</code>
  <br/>
  <blockquote>
  networks:
    my-proxy-net:
      external:
        name: custom_network
  </blockquote>
