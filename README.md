# MmsApiV2

## Introduction
This library for the [MMS API v2](https://developer.egym.com/docs/mms-v2/ncy7nz0n6bjdg-introduction) from EGYM provides an easy to use interface written in C#.
By using it you can create, delete and/or manage member accounts, checkins, checkouts, products, trainer tasks, webhooks and you can send push notifications.

## Features
* Targets .NET 8
* `async` by default
* MIT license
* Fully XML documented

## Installation

Install the latest version using dotnet CLI:

    > dotnet add package MmsApiV2

Install using Package Manager Console:

    > Install-Package MmsApiV2

## Example

```csharp
using System;
using MmsApiV2;

class Program
{
    static async Task Main()
    {
      var mmsApiV2Client = new MmsApiV2Client("YourApiKey");

      var memberAccount = await mmsApiV2Client.RetrieveAnAccount("AccountId");

      Console.WriteLine(memberAccount.FirstName);
    }
}
```