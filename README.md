# This Repository Is The Issue Demo Code

[Why the channels's write and read use same obj to lock? #47997](https://github.com/dotnet/runtime/issues/47997)

## How to Use
1. Clone.
1. Open the Channel_Lock_Question.sln(I create this project use Dotnet 5, and there has global.json if you want to use another version you can delete the global.json)
1. F5

## What happened?
You can use the ***Conditional compilation symbols*** to change this demo's operation and show diff state.    

#### Symbols:SendVersion
There has Three versions here.
1. SendVersion1
    this version use ***Task.Run*** to realization  the send message, if we use this version, no matter what client version we use, the code can not reach the line:105
1. SendVersion2
    this version sue ***Thread with IsBackground*** to realization the send message, if we use this version, all the client version can reach the line:105
1. SendVersion3 
    this version sue ***Task.Factory with long-running*** to realization the send message, if we use this version, all the client version can reach the line:105
#### Symbols:SendVersion
There have two versions here.
1. SendVersion1
    Get the write every time when we push the message to the channel.
2. SendVersin2
    Cache the write instance when the Proxy construct.

### Channel option or create type(bounded/undounded)
It has no impact on this issue
