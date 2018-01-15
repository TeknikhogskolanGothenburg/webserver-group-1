
# Webserver Architecture

# How are your webserver build?

We open up a HttpListener Class that listens to our server "localhost:8080/"

The program starts by locating the Content-map and giving it the path by the Environment.CurrentDirectory-property. It's value is stored
in the variable "Root".

We create a List by using the Directory.GetFileSystemEntries-property looping through every file that begins with the root address.
The condition is that the last five characters contains a '.' (dot). That is to make sure that the List contains only files and no
subfolders. Five chars because the longest extention is four chars(.html). If the condition is false, that means that it's a subfolder,
and in the else-statment we create another loop to search for grandchildren-files in the same way as above.

With the List we create a Dictionary taking two String-parameters. We loop the "Root"-address to extract the Substring after the word
"Content", leaving only e.g. \\anotherpage.htm , and then Replacing the "\\" to a "/", leaving "/anotherpage.htm". This will be our Key. 
The Value is its Root-value.

When someone sends a request we create an object and adds five things to it:
1. A StatusCode. 200 for correct web-addresses and 404 for incorrect. It is set by looping through our Dictionary for comparison to the
request.
2. A method is called to set the URL to the correct address. If it arrives empty = ((localhost:8080)/) it is set to its startpage 
((localhost:8080)/index.html). If it is a subfolder, we add its "child" to it, since there was only one (../subfolder/index.html). For 
every match with the Dictionary, the address stays the same. We use this output to compare Keys with our Dictionary. Its Value is
returned and saved in a Byte-array.
3. Taking the same output(Key) as above and send it into the MimeMapping.GetMimeMapping Method, to receive the ContentType.
4. Setting an expiring-date with the header-property one year from now.
5. Adding a Cookie to the CookieCollection and returning the name and Counter-value to the object.

A Dynamic page is available, that makes it possible for the user to make a query, e.g. ?input1=4&input2=5, returning the sum. 
A "/dynamic"-request with no variables in the query string, is responded with  StatusCode 500 = Missing input value.

These two object are added to the Response-class which then is added to a Stream output-object.
The user can see the output on the screen.

 
# What resources can the user access?

All present files in the Content-folder, having four character extentions or less. Also potential future files.

# How does the server act in case of an error?

If the error is due to network access or wrong host domain, an exeption is thrown.
If it is due to wrong input in the URL, a StatusCode is outputed to the user screen.
