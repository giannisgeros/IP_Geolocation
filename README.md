# The Project
Ip_Geolocation is a simple ASP.NET MVC 5 web application that searches the ip that you have given and returns some results.

## Components
- models (and validations with data annotations)
- web api calls
- asynchronus programming
- failovers
- services
- cache memory representation
- JavaScript / jQuery

## The Idea
The idea is to make a basic and simple example that includes some of the stuff that real world applications use. 

## Description
When an ip is entered at the input of the form, the app calls an API (ip2c) and returns the results. If the first API for some reason fails (you can try to block the API by redirecting it's ip), the app doesn't crash.
It tries to call a second API(ipStack) for the same results. There is not third API if the second fails but you get the idea :D.
When an ip is entered and the api returns results, the app writes these data to the cache for 3 seconds(just to see that they are there).
If someone tries to input the same ip again, the app doesn't call the api instead it takes the results from the cache.
This all works with JavaScript enabled (for better UX), but even with JavaScript turned off it still works.

## Installation
The second API (ipStack) requires an API key. 
Go make an account(it's free) and put your API key at the webconfig file, under configuration -> appsettings -> ipStackApiKey.
You may also have to restore some nuget packages.
Other than that, just plug and play.

## License
Distributed under the MIT license. See [LICENSE.md](LICENSE.md) for more information.