# YNAB

## Http
Http required abstractions to handle HTTP .NET

## Clients
Clients that handle direct communication with the YNAB API.

## Responses
Direct responses from the YNAB API.

## Wrappers
Named based on the data structure, obscures access to the YNAB API.

## Collections
Representations of grouped data.

# Commands
## Command
Taking given arguments to the handler.

## Command Handler
Using the command to orchestrate data into a view model then a console table.

# Compilation
Deciding how a data set is presented.

## Aggregates
Aggregates simplify data into reusable, simplfiied, data sets.

## Aggregator
Converts filtering parameters and data sources into a reusable data set of aggregates.

## ViewModelBuilder
Converts a data set into a view model, organised as necessary.

## ViewModel
Represents a set of rows and columns for a table.
