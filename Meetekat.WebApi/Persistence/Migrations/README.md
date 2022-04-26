# Migrations

Working with migrations in this project is quite different from the standard
approach, and there are reasons for that.

## Migrations rollback

First, consider the rollback aspect of the migration. Have you ever rollback a
rolled migration after a PR merge? Most likely, the answer to this question is
no. In this case, why implement the `Down` method for migration at all, since no
one uses it anyway?

## Designer files

Since we got rid of the `Down` method, why do we need `.Designer.cs` files then?
`ContextModelSnapshot` is enough to generate new migrations. Problems may arise
in order to remove migrations (may be useful as part of the work on a feature,
i.e. before the PR merge), because if you delete all `.Designer.cs` files, then
the content of the `ContextModelSnapshot` file will be overwritten during the
migration removal. The solution is to either to return the old version of the
`ContextModelSnapshot` file with Git, or to store the `.Designer.cs` for the
latest migration. As you can see, the Git restore option was chosen, but the
alternative sounds adequate.

## SQL in migrations

You can also pay attention to the fact that the code in the migrations has been
replaced with sql scripts. This is the first step towards rewriting migrations
from C# to SQL.

## Plans and future improvements

Further development plans for the project also include the addition of migration
tests. Also the format of the migration ID is highly questionable. It is easy
enough to stumble upon a situation where one developer started on branch A and
created a migration, after which another developer on branch B created another
migration (both from a migration in the target branch), after which the PR from
branch B merged first. As a result, we have a situation where the migration in
branch A is built on the basis of not the last migration, but by identifier
(which, by default, the timestamp of the moment the migration was created) comes
before the just merged migration + the situation can be reversed, and the
migration in branch A can be was created after the creation of the migration in
branch B, but before the PR merge with branch B. I haven't yet found a good
solution to this problem, but when I find it, the approach for working with
migrations can change significantly.
