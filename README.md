# XmlSerializerWriters
XmlSerializer with reflection in .NET Core 2.2 affects character escaping &amp; creates invalid XML

Our .NET Core 2.2 application would start generating invalid XML when the XmlSerializer was overidden to use reflection instead of RefEmit.  Not knowing this, I created a series of methods for serializing XML using different writers & settings.  I eventually found that XDocument worked correctly & I could use it's Writer with XmlSerializer.

This project contains unit tests for each Writer I could find.  It shows the failures on .NET Core 2.2 when reflection is enabled.  It also can be changed to a .NET Core 3.1 project to show that the issue is now resolved by re-running the tests.
