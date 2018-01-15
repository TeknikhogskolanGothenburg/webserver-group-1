
# HTTP version 2

What is new in HTTP version 2?

HTTP2 is now supported in most current releases of Edge, Safari, Firefox and Chrome. 

Improvments in this version is:

1. It is binary, instead of textual.
2. It is fully multiplexed, instead of ordered and blocking.

* This means that HTTP/2 can send multiple requests for data in parallel over a single TCP connection. This is the most advanced feature of the HTTP/2 protocol because it allows you to download web files via ASync mode from one server. Most modern browsers limit TCP connections to one server.

3. It can therefore use one connection for parallelism.
4. It uses header compression to reduce overhead.
5. It allows servers to “push” responses proactively into client caches instead of waiting for a new request for each resource.
6. It uses the new ALPN extension which allows for faster encrypted connections since the application protocol is determined during the initial connection.
7. It reduces additional round trip times (RTT), making your website load faster without any optimization.
8. Domain sharding and asset concatenation are no longer needed.

https://http2.github.io/faq/#what-does-http2-do-to-improve-security
