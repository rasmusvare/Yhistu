# Yhistu

## Docker
~~~yml
....
~~~

## Scaffolding

### DB Migrations

~~~bash
dotnet ef migrations --project App.DAL.EF --startup-project WebApp add Initial
dotnet ef migrations --project App.DAL.EF --startup-project WebApp remove Initial
dotnet ef database --project App.DAL.EF --startup-project WebApp update
dotnet ef database --project App.DAL.EF --startup-project WebApp drop
~~~

### Controllers

#### MVC razor based
~~~bash
cd WebApp

dotnet aspnet-codegenerator controller -name AssociationsController -actions -m App.Domain.Association -dc App.DAL.EF.AppDbContext -outDir Controllers --useAsyncActions --useDefaultLayout --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ApartmentsController -actions -m App.Domain.Apartment -dc App.DAL.EF.AppDbContext -outDir Controllers --useAsyncActions --useDefaultLayout --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name BuildingsController -actions -m App.Domain.Building -dc App.DAL.EF.AppDbContext -outDir Controllers --useAsyncActions --useDefaultLayout --referenceScriptLibraries -f

//Use areas:
dotnet aspnet-codegenerator controller -name MeetingOptionsController -actions -m App.Domain.MeetingOption -dc App.DAL.EF.AppDbContext -outDir Areas/Admin/Controllers --useAsyncActions --useDefaultLayout --referenceScriptLibraries -f
~~~

#### Web API
~~~bash
cd WebApp

dotnet aspnet-codegenerator controller -name ApartmentsController -actions -m App.Domain.Apartment -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name AssociationsController -actions -m App.Domain.Association -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name BankAccountController -actions -m App.Domain.BankAccount -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name BuildingsController -actions -m App.Domain.Building -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name CalculationRulesController -actions -m App.Domain.CalculationRules -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name ContactController -actions -m App.Domain.Contact -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name ContactTypeController -actions -m App.Domain.ContactType -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name InvoiceController -actions -m App.Domain.Invoice -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name InvoiceRowController -actions -m App.Domain.InvoiceRow -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name InvoiceTypeController -actions -m App.Domain.InvoiceType -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name MeasuringUnitController -actions -m App.Domain.MeasuringUnit -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name MemberController -actions -m App.Domain.Member -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name MemberTypeController -actions -m App.Domain.MemberType -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name MessageController -actions -m App.Domain.Message -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name MeterController -actions -m App.Domain.Meter -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name MeterReadingController -actions -m App.Domain.MeterReading -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name MeterTypeController -actions -m App.Domain.MeterType -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name PerkController -actions -m App.Domain.Perk -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name PerkTypeController -actions -m App.Domain.PerkType -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name PersonController -actions -m App.Domain.Person -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name RelationshipTypeController -actions -m App.Domain.RelationshipType -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name ServiceController -actions -m App.Domain.Service -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name ServiceProviderController -actions -m App.Domain.ServiceProvider -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name ServiceTypeController -actions -m App.Domain.ServiceType -dc App.DAL.EF.AppDbContext -outDir ApiControllers -api --useAsyncActions -f
~~~
