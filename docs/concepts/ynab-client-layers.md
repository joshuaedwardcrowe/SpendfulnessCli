# YNAB client layers

The layers between raw HTTP and the data the rest of the CLI consumes.

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
