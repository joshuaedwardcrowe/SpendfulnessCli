# Storing Data

## Premise
One of the key things I think is important to evaluate is what defines 'exceptional circumstances' within the application.

## Problem
This is currently unclear, with `Exceptions` exposed for a variety of reasons.

## Solution
My plan is to create a set of custom implementations of `Exception` for things that the user will typically run into, because I can safly `try`/`catch` around these custom impementations, and intentionally not handle things that are genuienly exceptional circumstances, such as incorrect usage of the concepts.

## Constraints
TBC
## Questions & Answers
TBC