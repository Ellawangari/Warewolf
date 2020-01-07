﻿Feature: RedisCache
	In order to avoid rerunning the work-flow every time we need generated data
	As a user
	I want to be to cached data while the Time To Live has not elapsed 

@RedisCache
Scenario: No data in cache
	Given Redis source "192.168.104.19" with password "pass123" and port "6379"
	And I have a key "MyData" and ttl of "1" seconds
	And No data in the cache
	And an assign "dataToStore" as
		| var      | value   |
		| [[Var1]] | "Test1" |
	When I execute the Redis Cache tool
	Then the cache will contain
		| Key    | Data             |
		| MyData | "[[Var1]],Test1" |
	And output variables have the following values
		| var      | value   |
		| [[Var1]] | "Test1" |

@RedisCache
Scenario: Data exists for given TTL not hit
	Given Redis source "192.168.104.19" with password "pass123" and port "6379"
	And I have a key "MyData" and ttl of "1" seconds
	And data exists (TTL not hit) for key "MyData" as
		| Key    | Data                     |
		| MyData | "[[Var1]],Data in cache" |
	And an assign "dataToStore" as
		| var      | value   |
		| [[Var1]] | "Test1" |
	When I execute the Redis Cache tool
	Then the assign "dataToStore" is not executed
	And output variables have the following values
		| var      | value                    |
		| [[Var1]] | "[[Var1]],Data in cache" |

@RedisCache
Scenario: Data Not Exist For Given Key (TTL exceeded) Spec
	Given Redis source "192.168.104.19" with password "pass123" and port "6379"
	And I have a key "MyData" and ttl of "1" seconds
	And data does not exist (TTL exceeded) for key "MyData" as
		| | |
	And an assign "dataToStore" as
		| var      | value   |
		| [[Var1]] | "Test1" |
	When I execute the Redis Cache tool
	Then the assign "dataToStore" is executed 
	Then the cache will contain
		| Key    | Data             |
		| MyData | "[[Var1]],Test1" |
	And output variables have the following values
		| var      | value   |
		| [[Var1]] | "Test1" |


@RedisCache
Scenario: Input Variable Keys Are Less Then Cached Data Variable Keys
	Given Redis source "192.168.104.19" with password "pass123" and port "6379"
	And I have "key1" of "MyData" and "ttl1" of "15" seconds
	And I have "key2" of "MyData" and "ttl2" of "3" seconds
	And an assign "dataToStore1" into "DsfMultiAssignActivity1" with
		| name      | value  |
		| [[Var1]] | "Test1" |
		| [[Var2]] | "Test2" |
	And an assign "dataToStore2" into "DsfMultiAssignActivity2" with
		| name      | value   |
		| [[Var1]] | "Test21" |
	Then the assigned "key1", "ttl1" and innerActivity "DsfMultiAssignActivity1" is executed by "RedisActivity1"
	And the Redis Cache under "MyData" will contain
		| name     | value   |
		| [[Var1]] | "Test1" |
		| [[Var2]] | "Test2" |
	Then the assigned "key2", "ttl2" and innerActivity "DsfMultiAssignActivity2" is executed by "RedisActivity2"
	Then "RedisActivity2" output variables have the following values
		| label							| variable		| operator	| value		|
		|	Redis key { MyData } found	|  null			|			|			|
		|			null				| [[Var1]]		|	 =		| "Test1"	|


Scenario: Input Variable Keys Are Greater Then Cached Data Variable Keys
	Given Redis source "192.168.104.19" with password "pass123" and port "6379"
	And I have "key1" of "MyData" and "ttl1" of "15" seconds
	And I have "key2" of "MyData" and "ttl2" of "3" seconds
	And an assign "dataToStore1" into "DsfMultiAssignActivity1" with
		| name      | value   |
		| [[Var1]] | "Test1" |
		| [[Var2]] | "Test2" |
	And an assign "dataToStore2" into "DsfMultiAssignActivity2" with
		| name      | value   |
		| [[Var1]] | "Test21" |
		| [[Var2]] | "Test22" |
		| [[Var3]] | "Test23" |
		| [[Var4]] | "Test24" |
	Then the assigned "key1", "ttl1" and innerActivity "DsfMultiAssignActivity1" is executed by "RedisActivity1"
	And the Redis Cache under "MyData" will contain
		| name     | value   |
		| [[Var1]] | "Test1" |
		| [[Var2]] | "Test2" |
	Then the assigned "key2", "ttl2" and innerActivity "DsfMultiAssignActivity2" is executed by "RedisActivity2"
	Then "RedisActivity2" output variables have the following values
	#And the debug output has "cached data missing key:[[Var3]]" error
	#And the debug output has "cached data missing key:[[Var4]]" error