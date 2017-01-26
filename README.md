# DynamicStyleBundles
A lightweight library which simplifies dynamic assets bundling via System.Web.Optimization  
[![Build status](https://ci.appveyor.com/api/projects/status/95u4qhpatb3kvj7m/branch/master?svg=true)](https://ci.appveyor.com/project/ogaudefroy/dynamicstylebundles/branch/master)

## Core Features
 - Delivers dynamic style assets and bundles them (use case: white label sites)
 - Feature toggled cached bundling 
 - Supports cache dependencies
 - Multi tenancy support


### Registering your dynamic assets

###Feature toggled cached bundling
A common scenario when building white label sites is the capability to provide a UI where technical users can update stylesheets, images and fonts. As a consequence when applying dynamic assets bundling it can be very convenient to temporary disable the bundling only for a specific user in order to test its updates.

This library implements this requirement with the ICacheToggleProvider interface which creates an extensibility point where you can implement your own feature toggle logic.

A default toggle provider is provided if no provider is registered with the bundle: DefaultCacheToggleProvider which always activates caching.

###Multi tenancy support
By default DynamicStyleBundles supports multitenancy through its caching mechanism which generates caching keys prefixed with HTTP_HOST server variable. This behavior is implemented in DefaultCacheKeyGenerator class. 

If this design doesn't match your use case you can create your own cache key generator by implementing ICacheKeyGenerator and registering your implementation when instantiating your DynamicStyleBundle.

###Cache dependencies
It can also be very convenient to be able to automatically refresh your bundle when an asset is edited. By default, DynamicStyleBundles provides a TimeSpanCacheDependency which expires after 15 minutes. 

You can provide an alternate implementation by implementing the ICacheDependencyBuilder interface and register it when setting the virtual path provider.