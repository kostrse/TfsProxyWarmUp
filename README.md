TFS Proxy warm up tool
==

The application provides a simple way to prefetch frequently used projects and files from a remote TFS server
by forcing them to be downloaded by a TFS Proxy server.

This can be extremely useful for distributed development teams working in different time zones.

The application doesn't require configuration of any TFS workspaces and even doesn't save downloaded files
to a local drive, so it can be easily configured as a daily batch job on server or a developer's workstation
using Windows Task Scheduler.

