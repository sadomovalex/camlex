# Camlex.Net - Home

Project was moved from Codeplex as last one will be shut down in Dec 2017.

## Project Description
Camlex.NET simplifies creating of CAML queries for Windows SharePoint Services by using expression trees. Compiled binaries can be downloaded from Nuget: [Camlex.NET](https://www.nuget.org/packages/Camlex.NET.dll/)

If you find Camlex.NET useful, you may support its development via ![](https://github.com/sadomovalex/camlex/blob/master/docs/Home_btn_donate_SM.gif) button on the online version http://camlex-online.org.

## Client object model
Version for Client object model is also available. It can be downloaded also from Nuget: [Camlex.Client](https://www.nuget.org/packages/Camlex.Client.dll/).

## Online version
You can convert your existing CAML queries to C# code for Camlex.NET using online service powered by Camlex: [http://camlex-online.org](http://camlex-online.org).
![powered by ndepend](Home_ndepend.png|http://www.ndepend.com)

## Installation Instructions
In order to start working with Camlex.NET, please download and reference Camlex.NET.dll assembly in your project. After that you can create CAML queries using lambda expressions.
Starting from version 3.0 you can install NuGet package for Camlex from online packages gallery:
{{
Install-Package Camlex.NET.dll
}}
or Client version:
{{
Install-Package Camlex.Client.dll
}}

## Quick Start
Camlex.NET is a new technique in Sharepoint development. It abstracts developers from syntax of CAML queries and helps them to concentrate on business tasks. With Camlex.NET developers could think about WHAT they need to do, instead of HOW to do it. It also brings the following advantages for Sharepoint developers:
* Compile time checking of expressions
* Natural and intuitive syntax using lambda expressions and fluent interfaces
* Support for native .Net types (int, strings, bool, DateTime, Guid) and operations (==, !=, >, <, etc)
* Support for Sharepoint-specific data types
* Ability to specify non-constant expressions inside filtering conditions (variables, method calls, etc)
* Fully customizable resulting query object
* Ability to build dynamic filtering conditions and join them using And/Or logical joins
* Search by lookup fields using LookupValue or LookupId

Now let's consider some basic scenarios:

**Scenario 1. Simple query**
Suppose that you need to select all items which have Status field set to Completed (following is the standard syntax of CAML):
{code:xml}
<Where>
  <Eq>
    <FieldRef Name="Status" />
    <Value Type="Text">Completed</Value>
  </Eq>
</Where>
{code:xml}
This query can be made with Camlex using the following syntax:
{code:c#}
string caml =
    Camlex.Query()
        .Where(x => (string)x["Status"](_Status_) == "Completed").ToString();
{code:c#}
Notice, other comparison operations like “<”, “<=”, “>”, “>=” are supported as well.

**Scenario 2. Query with “and”/”or” conditions**
Suppose that you need to select items which have ProductID = 1000 and IsCompleted set to false or null. Syntax of appropriate standard CAML query follows:
{code:xml}
<Where>
  <And>
    <Eq>
      <FieldRef Name="ProductID" />
      <Value Type="Integer">1000</Value>
    </Eq>
    <Or>
      <Eq>
        <FieldRef Name="IsCompleted" />
        <Value Type="Boolean">0</Value>
      </Eq>
      <IsNull>
        <FieldRef Name="IsCompleted" />
      </IsNull>
    </Or>
  </And>
</Where>
{code:xml}
With help of Camlex it could be converted using following natural syntax:
{code:c#}
var caml =
    Camlex.Query()
        .Where(x => (int)x["ProductID"](_ProductID_) == 1000 && ((bool)x["IsCompleted"](_IsCompleted_)(_IsCompleted_) == false || x["IsCompleted"](_IsCompleted_)(_IsCompleted_) == null))
            .ToString();
{code:c#}

**Scenario 3. Query with DateTime**
Lets suppose that you need to retrieve items which were modified on 01-Jan-2010:
{code:xml}
<Where>
  <Eq>
    <FieldRef Name="Modified" />
    <Value Type="DateTime">2010-01-01T12:00:00Z</Value>
  </Eq>
</Where>
{code:xml}
Using Camlex you can simply write:
{code:c#}
var caml =
    Camlex.Query()
        .Where(x => (DateTime)x["Modified"](_Modified_) == new DateTime(2010, 01, 01)).ToString();
{code:c#}

**Scenario 4. Query with BeginsWith and Contains operations**
Consider the query that should return items which Title field starts with “Task” and Project field contains “Camlex”:
{code:xml}
<Where>
  <And>
    <BeginsWith>
      <FieldRef Name="Title" />
      <Value Type="Text">Task</Value>
    </BeginsWith>
    <Contains>
      <FieldRef Name="Project" />
      <Value Type="Text">Camlex</Value>
    </Contains>
  </And>
</Where>
{code:xml}
You can achieve result using the following natural syntax:
{code:c#}
var caml =
    Camlex.Query()
        .Where(x => ((string)x["Title"](_Title_)).StartsWith("Task") && ((string)x["Project"](_Project_)).Contains("Camlex"))
            .ToString();
{code:c#}

**Scenario 5. Query with none C# native data types**
Suppose that you need to retrieve all items modified by Administrator:
{code:xml}
<Where>
  <Eq>
    <FieldRef Name="Editor" />
    <Value Type="User">Administrator</Value>
  </Eq>
</Where>
{code:xml}
Notice that value is of Sharepoint-specific User data type. You can easily achieve the result with alternative string-based syntax:
{code:c#}
var caml =
    Camlex.Query()
        .Where(x => x["Editor"](_Editor_) == (DataTypes.User)"Administrator")
                .ToString();
{code:c#}

**Scenario 6. Query with sorting (OrderBy)**
Suppose that you need to select all items which have ID >= 5 and the result should be sorted by Modified field:
{code:xml}
<Where>
  <Geq>
    <FieldRef Name="ID" />
    <Value Type="Integer">5</Value>
  </Geq>
</Where>
<OrderBy>
  <FieldRef Name="Modified" />
</OrderBy>
{code:xml}
You need to write the following Camlex expression in order to construct this query:
{code:c#}
var caml =
    Camlex.Query()
        .Where(x => (int)x["ID"](_ID_) >= 5)
        .OrderBy(x => x["Modified"](_Modified_)).ToString();
{code:c#}

**Scenario 7. Query with grouping (GroupBy)**
Suppose that we need to select items having not-null Status field and result set should be grouped by Author field:
{code:xml}
<Where>
  <IsNotNull>
    <FieldRef Name="Status" />
  </IsNotNull>
</Where>
<GroupBy>
  <FieldRef Name="Author" />
</GroupBy>
{code:xml}
With Camlex you could simply rewrite it as:
{code:c#}
var caml =
    Camlex.Query()
        .Where(x => x["Status"](_Status_) != null)
        .GroupBy(x => x["Author"](_Author_)).ToString();
{code:c#}

**Scenario 8. Query with non-constant expressions in lvalue and rvalue**
Non-constant expression gives you more control over CAML. Suppose that you need to select items depending on current locale: for English locale you need to select items which have TitleEng field set to “eng”; for non-English locale you need to select items which have Title field set to “non-eng”. I.e.:
Query for English locale:
{code:xml}
<Where>
  <Eq>
    <FieldRef Name="TitleEng" />
    <Value Type="Text">eng</Value>
  </Eq>
</Where>
{code:xml}
Query for non-English locale:
{code:xml}
<Where>
  <Eq>
    <FieldRef Name="Title" />
    <Value Type="Text">non-eng</Value>
  </Eq>
</Where>
{code:xml}
It is not so hard with Camlex:
{code:c#}
bool isEng = true; // or false depending on Thread.CurrentThread.CurrentUICulture

var caml =
    Camlex.Query()
        .Where(x => (string)x[isEng ? "TitleEng" : "Title"](isEng-_-_TitleEng_-_-_Title_) == (isEng ? "eng" : "non-eng")).ToString();
{code:c#}

**Scenario 9. Dynamic filtering conditions**
Starting with 2.0 version you can build dynamic filtering conditions and join them using WhereAll/WhereAny methods which use And/Or logical joins respectively.
Suppose that we need to retrieve all items which contain at least one of the values {“hello”, “greeting”, “hi”} in Title field. I.e. we need to use the following CAML query:
{code:xml}
<Where>
  <Or>
    <Or>
      <Contains>
        <FieldRef Name="Title" />
        <Value Type="Text">hello</Value>
      </Contains>
      <Contains>
        <FieldRef Name="Title" />
        <Value Type="Text">greeting</Value>
      </Contains>
    </Or>
    <Contains>
      <FieldRef Name="Title" />
      <Value Type="Text">hi</Value>
    </Contains>
  </Or>
</Where>
{code:xml}
With Camlex.NET 2.0 we can create lambda expression for each condition and pass them into new WhereAny method:
{code:c#}
// list of tokens
var tokens = new List<string> { "hello", "greeting", "hi" };
var expressions = new List<Expression<Func<SPListItem, bool>>>();

// create lambda expression for each token in list
foreach (string t in tokens)
{
    string token = t;
   expressions.Add(x => ((string)x["Title"](_Title_)).Contains(token));
}

// prepare query
var caml = Camlex.Query().WhereAny(expressions).ToString();
{code:c#}

**Note**: in the example above it is **important** to create local variable "string token" inside loop body and assign current token to it. Without it resulting query will use only last token for all conditions. This is because of lazy expressions implementation in .Net 3.5).

Also it is possible to create more complex dynamic CAML queries using ExpressionsHelper class which allows to combine using Or or And operations other existing conditions:
{code:c#}
public static class ExpressionsHelper
{
    public static Expression<Func<SPListItem, bool>> CombineAnd(
        IEnumerable<Expression<Func<SPListItem, bool>>> expressions)
    {
        ...
    }
 
    public static Expression<Func<SPListItem, bool>> CombineOr(
        IEnumerable<Expression<Func<SPListItem, bool>>> expressions)
    {
        ...
    }
}
{code:c#}

It can be used like that:
{code:c#}
// Language = Russian or Language = English
var languageConditions = new List<Expression<Func<SPListItem, bool>>>();
languageConditions.Add(x => (string)x["Language"](_Language_) == "Russian");
languageConditions.Add(x => (string)x["Language"](_Language_) == "English");
var langExpr = ExpressionsHelper.CombineOr(languageConditions);
 
// FileLeafRef contains “.docx” or FileLeafRef contains “.xlsx” or ...
var extenssionsConditions = new List<Expression<Func<SPListItem, bool>>>();
var extensions = new[]() {".docx", ".xlsx", ".pptx"};
foreach (string e in extensions)
{
    string ext = e;
    extenssionsConditions.Add(x => ((string)x["FileLeafRef"](_FileLeafRef_)).Contains(ext));
}
var extExpr = ExpressionsHelper.CombineOr(extenssionsConditions);
 
// (Language = Russian or Language = English) and
// (FileLeafRef contains “.docx” or FileLeafRef contains “.xlsx” or ...)
var expressions = new List<Expression<Func<SPListItem, bool>>>();
expressions.Add(langExpr);
expressions.Add(extExpr);

string query = CamlexNET.Camlex.Query().WhereAll(expressions).ToString();
{code:c#}

In result we will get the following CAML:
{code:xml}
<Where>
  <And>
    <Or>
      <Eq>
        <FieldRef Name="Language" />
        <Value Type="Text">Russian</Value>
      </Eq>
      <Eq>
        <FieldRef Name="Language" />
        <Value Type="Text">English</Value>
      </Eq>
    </Or>
    <Or>
      <Or>
        <Contains>
          <FieldRef Name="FileLeafRef" />
          <Value Type="Text">.docx</Value>
        </Contains>
        <Contains>
          <FieldRef Name="FileLeafRef" />
          <Value Type="Text">.xlsx</Value>
        </Contains>
      </Or>
      <Contains>
        <FieldRef Name="FileLeafRef" />
        <Value Type="Text">.pptx</Value>
      </Contains>
    </Or>
  </And>
</Where>
{code:xml}

**Scenario 10. List joins**
Starting with version 4.0 (and Camlex.Client 2.0) it became possible to create list joins and fields projections:
{code:c#}
var query = new SPQuery();
 
query.Query = Camlex.Query().Where(x => (string)x["CustomerCity"](_CustomerCity_) == "London" &&
    (string)x["CustomerCityState"](_CustomerCityState_) == "UK").ToString();
 
query.Joins = Camlex.Query().Joins()
    .Left(x => x["CustomerName"](_CustomerName_).ForeignList("Customers"))
    .Left(x => x["CityName"](_CityName_).PrimaryList("Customers").ForeignList("CustomerCities"))
    .Left(x => x["StateName"](_StateName_).PrimaryList("CustomerCities").ForeignList("CustomerCityStates"))
    .ToString();
 
query.ProjectedFields = Camlex.Query().ProjectedFields()
    .Field(x => x["CustomerCity"](_CustomerCity_).List("CustomerCities").ShowField("Title"))
    .Field(x => x["CustomerCityState"](_CustomerCityState_).List("CustomerCityStates").ShowField("Title"))
    .ToString();
 
query.ViewFields = Camlex.Query().ViewFields(x => new[]() {x["CustomerCity"](_CustomerCity_),
    x["CustomerCityState"](_CustomerCityState_)});
{code:c#}

In this example we will get the following CAML for different SPQuery properties:
Query:
{code:xml}
<Where>
  <And>
    <Eq>
      <FieldRef Name="CustomerCity" />
      <Value Type="Text">London</Value>
    </Eq>
    <Eq>
      <FieldRef Name="CustomerCityState" />
      <Value Type="Text">UK</Value>
    </Eq>
  </And>
</Where>
{code:xml}

Joins:
{code:xml}
<Join Type="LEFT" ListAlias="Customers">
  <Eq>
    <FieldRef Name="CustomerName" RefType="Id" />
    <FieldRef List="Customers" Name="Id" />
  </Eq>
</Join>
<Join Type="LEFT" ListAlias="CustomerCities">
  <Eq>
    <FieldRef List="Customers" Name="CityName" RefType="Id" />
    <FieldRef List="CustomerCities" Name="Id" />
  </Eq>
</Join>
<Join Type="LEFT" ListAlias="CustomerCityStates">
  <Eq>
    <FieldRef List="CustomerCities" Name="StateName" RefType="Id" />
    <FieldRef List="CustomerCityStates" Name="Id" />
  </Eq>
</Join>
{code:xml}

ProjectedFields:
{code:xml}
<Field Name="CustomerCity" Type="Lookup" List="CustomerCities" ShowField="Title" />
<Field Name="CustomerCityState" Type="Lookup" List="CustomerCityStates" ShowField="Title" />
{code:xml}

ViewFields:
{code:xml}
<FieldRef Name="CustomerCity" />
<FieldRef Name="CustomerCityState" />
{code:xml}

**Scenario 11. Support for LookupMulti field type**
Starting with version 4.2 (and Camlex.Client 2.2) LookupMulti field type is supported (with possibility to specify LookupId="True" in FieldRef):
{code:c#}
var caml =
    Camlex.Query()
        .Where(x => x["Title"](_Title_) > (DataTypes.LookupMultiId)"5"
        && x["Author"](_Author_) == (DataTypes.LookupMultiValue)"Martin").ToString();
{code:c#}

This example will produce the following CAML:
{code:xml}
<Where>
  <And>
    <Gt>
      <FieldRef Name="Title" LookupId="True" />
      <Value Type="LookupMulti">5</Value>
    </Gt>
    <Eq>
      <FieldRef Name="Author" />
      <Value Type="LookupMulti">Martin</Value>
    </Eq>
  </And>
</Where>
{code:xml}
