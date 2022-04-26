# Application Features

The cornerstone of the entire application.

## Overview

The `Features` namespace contains all application logic grouped by sections.
Each feature is **self-sufficient** and must **not** depend on any other
features. Try to reuse the code between different features **as little as
possible** (yes, this is a serious violation of the DRY principle and this is
**the most important** aspect of the chosen application architecture).

## Reasoning behind the chosen approach

### Brief introduction

In contrast to the N-Tier (a.k.a. Layered) architecture that has already become
the norm, I propose to first divide the application into vertical functional
pillars and only then (if necessary) consider the issue of dividing each
individual pillar into layers.

### Comparison with N-Tier

#### The problem we're trying to solve

To understand the advantages of this approach, let's first take look at a basic
form of building an application that everyone is definitely familiar with and
that undoubtedly has huge drawbacks: *1-Tier architecture*.

Suppose we use an approach where we have a 1:1 relationship between entities in
the database (perhaps with the exception of intermediate tables required to
organize many-to-many relationships) and controllers (exactly 1 controller per
entity - no more , no less).

In this case, we sooner or later come to the conclusion that we have dozens of
controllers in our application, which are simply unbearable to work with because
of their size (which is thousands of lines, sometimes exceeding dozens of
thousands).

#### What does N-Tier bring to the table

And what do we do with such controllers? That's right, we separate them  into 3
tiers (a.k.a layers):
1. We leave the external shell of the `Controller`, in which remain only the
mappings between the HTTP protocol and the internals of our application,
documentation comments and attributes (often used to generate Swagger
documentation based on them).
2. Separate the business logic of the application into `Services`.
3. We transfer all calls to the database from services to `Repositories` and
encapsulate the details of interaction with the database in them.

Well, in this case, the distribution of the number of lines is approximately the
following:
1. `Controllers` typically keep between 5 and 20 percent of the total amount of
code (thanks to documentation comments).
2. `Services` take most of the code: something like 70 percent (logical, since
most of the request processing logic settles there).
4. From a third to a half of all code goes into the `Repository` (interaction
with the database is also not an easy task).

Also note that after layering, the total amount of code has increased.

Has the situation changed dramatically for the better? If earlier we had one
`Controller` for 5k lines, now we will have a `Controller` for .5k lines, a
`Service` for 3.5k lines and a `Repository` for almost 2k lines. After all,
nothing has changed - we still just have huge code files that are impossible to
work with. Rather, on the contrary, we only got even more problems:
1. Now it is much more difficult for us to navigate the code.
2. Now different `Services` are interconnected.

First of all, when we need (for example) to fix a filtering parameter bug (the
filtering parameter is simply ignored), we will have to go through all 3 layers
in order to find out on which layer the problem occurred (we could not pass the
filtering parameter at the `Controller` level, forget to use it in the `Service`
method or incorrectly formed a request to the database at the `Repository`
level).

Also, when we pushed part of initially `Controller`'s code into `Repositories`,
we introduced the possibility for all other `Services` to reuse `Repository`
code. At first glance this seems like a pure benefit, but in fact we just
coupled our `Services` with each other: now in the `Repository` we will have to
support a method that meets the needs of several `Services` at once (and these
needs quite often differ significantly), or keep several distinct methods each
corresponding to a specific `Service` (then it's not clear why this code is in a
common repository, and not in a separate service).

#### Proposed solution

If we take a closer look at the original 5k-lines controller, we'll notice that
it has quite a lot of methods (for the given amount of code, it's usually
somewhere between 20 and 50 methods). But what happens if we just put each
method in a separate file/class? We get complete isolation between these methods
(unlike the mess that happens with repositories in the N-Tier approach) and
quite tolerable file sizes: 75-350 lines each (don't forget that when splitting
one file into several distinct files, the total amount code increases).

As a result, we get:
1. The absence of reusable feature-related code (remember the problem with
reusing repository methods).
2. Compliance with the SRP (due to which we always know that if, for example,
the filtering parameter does not work specifically for the dish resource, then
we need to look for the problem specifically in this feature, and not be
scattered in searches between the controller, service and several repositories).
3. Compliance with the OCP (implementation of a new functionality means adding a
new feature, fixing a bug in an existing functionality means fixing a specific
existing feature).

In addition, since we moved this feature into a separate class, nothing now
prevents us from dividing the original method into several different methods
(encapsulating part of the business-logic in them). In this case, you can move
away from 300-line methods towards methods of 50 or fewer lines (if you wish
so). You can even go further and split the class into several other classes (in
this case, the feature boundaries will be defined by the namespace).

### Comparison with Onion

#### Sometimes abstractions are absurd

The main difference from the Onion architecture is that I'm not paranoid about
dependencies. There is basically no need to abstract from everything in the
world, this is stupid. You need to start from real problems and constantly
evaluate what different abstractions give you (does it save you from any
problems, to what extent) and how much it costs.

#### Abstracting from ASP

What's the point of abstracting away from ASP? If you are planning to use
something other than ASP, then it is worth weighing the pros and cons of
introducing an additional abstraction. If you do not plan to use something other
than ASP, then you do not need abstraction. Think for yourself how much you need
to implement or customize to get the same thing that ASP gives you:
1. Own response system that does not depend on ASP and is not related to HTTP in
any way. This includes the mechanisms for generating and processing these
responses. Don't forget also that if you really want a reliable fully isolated
abstraction, then you will have to write quite a lot of boilerplate code, for
example, separate DTOs (you want to abstract from ASP, so you can only specify a
custom binder or JSON property name by creating a separate DTO class , which
will match your model in almost 100% of cases) or mappings from custom internal
responses to HTTP status codes.
2. Own middleware system (don't forget that you will also need to validate
models, log requests, catch unhandled exceptions etc. and for this it's better
to make a custom middleware pipelines system right away). In many cases, MediatR
library is used for this, but this doesn't negate the fact that in order to
build an abstraction, the middlewares system must also be implemented.
3. Connecting the DI system. You won't instantiate all services manually, will
you? Well, then screw DI yourself, since you decided to abstract from ASP.

This is only a small part of what you have to do to abstract from ASP. Now
answer the questions honestly: are you sure that abstracting from ASP without a
reason is a good idea?

#### Abstracting from EF Core

The same goes for EF Core. To achieve complete abstraction, you should never
expose a `DbContext`, `DbSet` or `IQueryable`. You mustn't provide access to the
query engine, which means that you are not allowed to use `.Include` and
`.ProjectTo` either (of course, only if you don't want to build your own
shittier version of `IQueryable` on top of the real one). You'll constantly
encounter under-fetching and over-fetching problems, repository methods will
either be simply monstrous in size (handling all possible scenarios for their
use), or absurdly granular (which will harm both code readability and
performance). Soon enough, dependence on EF Core will begin to leak in an
implicit form: you will begin to form your domain model based not on what is
convenient for you to use, but based on the EF Core limitations. Again, the
question: is introducing an really worth it?

#### Conclusion on abstractions

I'm not saying that abstractions are absolutely never needed. I'm just saying
that we need to weigh the pros and cons when we decide whether to build an
abstraction or not. In this application, we have abstracted away the mechanism
for issuing and validating JWT tokens, but are still dependent on the fact that
the authentication system is built on a two-token system. We didn't configure
the database schema configuration directly in the entities (instead of
Annotations, we used IEntityTypeConfiguration), but we didn't abstract from EF
Core itself.

## Feature sectioning

A rather important aspect is also the division of features into sections. At the
moment, the application is divided into 3 sections:
1. `Auth`. This section includes both the most general mechanisms (for example,
issuing a pair of tokens or changing username and password), as well as more
application-specific aspects (for example, roles).
2. `Feed`. Here the user can find meetups that are interesting to him, see
detailed information about them, discuss them with other users, ask clarifying
questions to the organizers, leave feedback after visiting.
3. `Studio`. In this section, registration, moderation and collection of
statistics on meetups takes place.

**Note**: not all functionality is implemented at the moment.

The division of these sections allows you to clearly distinguish between
different areas of activity on the project. It will be easier for you to find a
common language with customers and other developers, it will be easier to
navigate through the sections. Later on, along the boundaries of these sections,
you could divide your application into separate applications or microservices:
1. The `Auth` section can be completely replaced by an external authentication
system.
2. A library with general settings, configurations, solutions will be prepared
(the basis for this is already laid under the `Seedwork` namespace).
3. The `Feed` and `Studio` sections are moved to separate applications.
