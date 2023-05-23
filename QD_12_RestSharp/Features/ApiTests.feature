Feature: RestSharp
API tests performed for 2 endpoints: 
tag1 tests - reqres.in
tag2 tests - api.mathjs.org

@task1
Scenario Outline: Execute API tests - GET call
Given a base URL is https://reqres.in/api
When GET call executed to <endpoint>
Then the status code is <code>

Examples: 
| ScenarioName              | endpoint       | code |
| Get list users            | /users?page=2  | 200  |
| Get single user not found | /users/23      | 404  |
| Get single <2>            | /unknown/2     | 200  |
| Get delayed response      | /users?delay=3 | 200  |

@task1
Scenario Outline:  Execute API tests - POST/PUT/PATCH call
Given a base URL is https://reqres.in/api
When <method> is executed to <endpoint> using
| key    | value    |
| <key1> | <value1> |
| <key2> | <value2> |
Then the status code is <code>
And the following fields and values are in the response
| key    | value    |
| <key1> | <value1> |
| <key2> | <value2> |

Examples: 
| ScenarioName            | method             | endpoint     | code | key1  | value1       | key2 | value2        |
| Post create             | POST               | /api/users   | 201  | name  | morpheus     | job  | leader        |
| Put update              | PUT                | /api/users/2 | 200  | name  | morpheus     | job  | zion resident |
| Patch update            | PATCH              | /api/users/2 | 200  | name  | morpheus     | job  | zion resident |

@task1
Scenario Outline: Execute API tests - Register/Login/Delete
#login step response in spec is 400, but 201 received
Given a base URL is https://reqres.in/api
When <method> is executed to <endpoint> using
| key    | value    |
| <key1> | <value1> |
| <key2> | <value2> |
Then the status code is <code>

Examples: 
| ScenarioName             | method | endpoint      | code | key1  | value1             | key2     | value2        |
| Delete delete            | DELETE | /api/users/2  | 204  | name  | morpheus           | job      | zion resident |
| Post register successful | POST   | /api/register | 201  | email | eve.holt@reqres.in | password | pistol        |
| Post login unsuccessful  | POST   | /api/login    | 400  | email | peter@klaven       | -        | -             |


@task2
Scenario Outline: Math API - math operations
Given a base URL is http://api.mathjs.org/v4/
When <operation> action is executed with <expression>
Then the status code is <code>
And the <expectedResult> is in the response

Examples: 
| operation  | expression | expectedResult | code |
| Multiply   | 2*6        | 12             | 200  |
| Divide     | 10 / 2     | 5              | 200  |
| Add        | 4+5        | 9              | 200  |
| Subtract   | 8 - 3      | 5              | 200  |
| SquareRoot | sqrt(16)   | 4              | 200  |
