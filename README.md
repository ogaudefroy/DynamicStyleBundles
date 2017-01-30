# DynamicStyleBundles
A lightweight library which simplifies dynamic assets bundling via System.Web.Optimization  
[![Build status](https://ci.appveyor.com/api/projects/status/95u4qhpatb3kvj7m/branch/master?svg=true)](https://ci.appveyor.com/project/ogaudefroy/dynamicstylebundles/branch/master)

## Core Features
 - Delivers dynamic style assets and bundles them (use case: white label sites)
 - Feature toggled cached bundling 
 - Supports cache dependencies
 - Multi tenancy support
 
## Getting up and running
### Install
Install the nuget package ; the setup will add the reference and will tweak your web.config file to add a brand new handler definition.

`<add name="DynamicStyleBundles" path="DynamicContent/*" verb="GET" type="DynamicStyleBundles.HttpHandlerFactory, DynamicStyleBundles" preCondition="integratedMode" />`

### Configure the HttpHandler

 1. Modify the handler's path to match your needs

### Registering your dynamic bundles
Replace your StyleBundle instantiations by DynamicStyleBundle instantiations and you're up and running to deliver dynamic assets. 

DynamicStyleBundle do support CDN, transforms but also multi tenancy design and feature toggle caching (see below).

## Advanced scenarios  
###Feature toggled cached bundling
A common scenario when building white label sites is the capability to provide a UI where technical users can update stylesheets, images and fonts. As a consequence when applying dynamic assets bundling it can be very convenient to temporary disable the bundling only for a specific user in order to test its updates.

This library implements this requirement with the [ICacheToggleProvider](https://github.com/ogaudefroy/DynamicStyleBundles/blob/master/DynamicStyleBundles/ICacheToggleProvider.cs) interface which creates an extensibility point where you can implement your own feature toggle logic.

A default toggle provider is provided if no provider is registered with the bundle: [DefaultCacheToggleProvider](https://github.com/ogaudefroy/DynamicStyleBundles/blob/master/DynamicStyleBundles/DefaultCacheToggleProvider.cs) which always activates caching.

###Multi tenancy support
By default DynamicStyleBundles supports multitenancy through its caching mechanism which generates caching keys prefixed with HTTP_HOST server variable. This behavior is implemented in [DefaultCacheKeyGenerator](https://github.com/ogaudefroy/DynamicStyleBundles/blob/master/DynamicStyleBundles/DefaultCacheKeyGenerator.cs) class. 

If this design doesn't match your use case you can create your own cache key generator by implementing [ICacheKeyGenerator](https://github.com/ogaudefroy/DynamicStyleBundles/blob/master/DynamicStyleBundles/ICacheKeyGenerator.cs) and registering your implementation when instantiating your DynamicStyleBundle.

###Cache dependencies
It can also be very convenient to be able to automatically refresh your bundle when an asset is edited. By default, DynamicStyleBundle provides a [TimeSpanCacheDependency](https://github.com/ogaudefroy/DynamicStyleBundles/blob/master/DynamicStyleBundles/TimeSpanCacheDependency.cs) which expires after 15 minutes. 

You can provide an alternate implementation by implementing the [ICacheDependencyBuilder](https://github.com/ogaudefroy/DynamicStyleBundles/blob/master/DynamicStyleBundles/ICacheDependencyBuilder.cs) interface and register it when setting the virtual path provider.
