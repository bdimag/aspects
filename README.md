# aspects

examples of the `NotifyPropertyChanged` and `Requires` aspects:

```csharp
[NotifyPropertyChanged]
public string Third { get; set; }

[Requires(nameof(First), nameof(Second))]
public void DoWork()
{
    DoWorkResult = $"Values are {First}, {Second}.";
}
```

|Before|After|
|:-:|:-:|
![](without.gif)|![](with.gif)
