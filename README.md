# AttributeCollection

AttributeCollection allows you to add dynamic attributes in the inspector

![image](https://user-images.githubusercontent.com/4022114/198506575-c32f00a2-7934-4507-afd4-56c256c17fd5.png)

to read from the values:

```cs
Attribute<int> myAttribute = attributes.GetAttribute<Attribute<int>>("MyAttributeName");
```

or

```cs
if (attributes.TryGetAttribute("MyAttributeName", out AttributeBase attribute))
{
    var myAttribute = (Attribute<int>)attribute;
}
```
