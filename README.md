## Project Introduction
MyForum is my defense project for ASP.NET Core MVC course at SoftUni (April-June 2021). The website is..

## Overview
ASP.NET Core Template Author - 
<a href="https://github.com/NikolayIT" target="_blank">NikolayIT</a>
* Template for ASP.NET Core with repositories, services, models mapping, DI and StyleCop warnings fixed.

## Functionality
* Регистрация на потребители
* Възможност за разглеждане, създадване, четене на постове
* Възможност за коментиране, харесване, запазване и принтиране на постове
* Възможност за последване на потребители, както и да бъдете последвани от други потребители
* Възможност за филтриране по категория на постовете и търсене по заглавие
* Чат стая позволяваща на потребителите да обменят съобщения
* Кеширане на данните 'Memory Cache'(see below) в 'Home' страницата, с цел намаляне на заявките към базата данни
* Интерактивен, гъвкав UX

## Built With
* ASP.NET Core 5 MVC
* ASP.NET CORE view components
* ASP.NET Core areas
* MSSQL Server
* Entity Framework Core
* Auto-Mapper
* SignalR
* SendGrid
* AJAX
* jQuery
* Moq
* XUnit
* <a href="https://www.nuget.org/packages/X.PagedList.Mvc/">X.PagedList.Mvc</a>
* Bootstrap
* Font Awesome Icons

## Client Side Libraries
* TinyMCE
* Moment.js

## This is the current state of database diagrams relations in my app

![](DatabaseDiagrams.png)

## Testing
* 100% Tests of Services

![](ServicesCodeCoverage.png)

## Implementation

* Followings / Followers (Model)
```javascript
class UserFollow
{
    public string FollowerId { get; set; }

    public string FollowedId { get; set; }

    public bool IsFollowActive { get; set; }
}
// To see the rest of the functionality go on Follow(Service/Controller)
```

* Memory Cache
```javascript
if (!this.cache.TryGetValue<IEnumerable<HomePostViewModel>>("Posts",
out var posts))
{
    posts = this.postsService.GetAllPosts<HomePostViewModel>();

    var cacheEntryOptions = new MemoryCacheEntryOptions()
        .SetAbsoluteExpiration(TimeSpan.FromSeconds(100));

    this.cache.Set("Posts", posts, cacheEntryOptions);
}
```
* Print
```javascript
<a onclick="window.print()" class="btn btn-warning">
    <i class="fas fa-print"></i>&nbsp; Print
</a>
```

### Контрол на достъпа
* Гостите могат да се регистрират, да преглеждат каталога, детайлите на тестовете и профилните страници на потребителите
* Регистрираните потребители могат да решават тестове, да преглеждат резултатите си и да създават и редактират тестове
* Само създателя на един тест може да го редактира и изтрива
* Всеки регистриран потребител може да решава чужд тест