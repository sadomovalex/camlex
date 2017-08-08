The following are the features which are to be implemented in the next version of Camlex.NET project:
~~* Reverse engineering from pure CAML queries to C#-based Camlex queries~~ -done
~~* Camlex.Net for Client OM - http://camlex.codeplex.com/workitem/13989~~ -done
* Support for MEMBERSHIP nodes of CAML language
* Support for special forms for DATETIME values: <Value Type="DateTime">{"[Today + 7](Today-+-7)"}</Value> and <Value Type="DateTime"><Today /></Value> (http://camlex.codeplex.com/discussions/270483)
~~* Support for UserID in queries by user - see http://camlex.codeplex.com/discussions/264821~~ - done
* Support for using Camlex.NET project in Silverlight applications
* Support for NULLABLE attribute of CAML language - [http://msdn.microsoft.com/en-us/library/microsoft.sharepoint.spsitedataquery.viewfields.aspx](http://msdn.microsoft.com/en-us/library/microsoft.sharepoint.spsitedataquery.viewfields.aspx)
* Support for Taxonomy fields (only for SharePoint 2010) - [http://msdn.microsoft.com/en-us/library/ff625182.aspx](http://msdn.microsoft.com/en-us/library/ff625182.aspx), e.g. for TaxonomyFieldTypeMulti - [https://camlex.codeplex.com/workitem/13998](https://camlex.codeplex.com/workitem/13998)
* Support for float, double and decimal native data types
* Support more features available in CAML (e.g. <ProjectProperty> element inside <Value> tag)
* Compiled queries for better performance - see http://camlex.codeplex.com/discussions/330815
~~* No explicit comparisons for boolean fields: [http://camlex.codeplex.com/workitem/13991?ProjectName=camlex](http://camlex.codeplex.com/workitem/13991?ProjectName=camlex)~~
~~* Add conditions to existing string query - [http://camlex.codeplex.com/discussions/353158](http://camlex.codeplex.com/discussions/353158)~~ -done
~~* Combine several string queries to one - [http://camlex.codeplex.com/discussions/431325](http://camlex.codeplex.com/discussions/431325)~~
* Use strongly typed classes in queries: [http://camlex.codeplex.com/discussions/438324#post1022401](http://camlex.codeplex.com/discussions/438324#post1022401)
~~* Support for SP2010 operators: IN,INCLUDES,NOTINCLUDES~~
~~* Use Camlex.Client in Sharepoint 2013 apps (see https://camlex.codeplex.com/discussions/455242)~~ - use assembly binding redirection in app.config or rebuilt the source code against new .Net and Sharepoint versions
* Possibility to specify LookupId for In operator (see comments for http://sadomovalex.wordpress.com/2013/09/01/support-in-operation-%D0%B2-camlex-35/. Before to implement, check that such CAML will work)
~~* Support for LookupMulti type: https://camlex.codeplex.com/workitem/14011~~ -done
* Possibility to specify Override attribute for OrderBy: https://camlex.codeplex.com/workitem/14012