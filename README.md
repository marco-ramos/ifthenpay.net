# ifthenpay.net

This project allows you generate valid ATM references using IfThenPay.

Source: https://ifthenpay.com/?lang=en

### Note:
Before using this helper you need to have an valid account/contract in IfThenPay and obtain an *Entidade* and *SubEntidade* code.

## How to Use:
Create a configuration instance using *Entidade* and *SubEntidade* code:
```
var configuration = new Ifthenpay.Domain.Configuration("11111", "222")
```

Use the Generator Service to obtain a valid ATM based on a unique Id and the amount desired.
```
var atmReference = Ifthenpay.Services.Generator.GetReference(configuration, 1, 10);
```