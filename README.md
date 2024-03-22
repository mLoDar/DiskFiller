<br>

<h1 align="center">
  DiskFiller
</h1>

---

> :warning: **Disclaimer** :warning:

> This application was created for educational and learning purpose and is not intended to be abused in any form.

> I am not responsible for any damage or system failure caused by this application.
Only use it on your own systems or with explicit permission of a devices owner.

> The application hides itself from the taskbar and has no windows. It is closable by the Task Manager.

---

<br>

## How it works:

- The application checks if there are at least 4GB of free space.

- After that the application copies itself to `%appdata%`. Once copied there, the current process will be closed and the file will be deleted.

- Next step is creating a folder within `%appdata%` which contains a GUID. That folder will be used during runtime for the creation of new files.

- Each file contains ~500MB of data.

- There are 5 Threads in total which each create multiple files per second.

<br>

---

<br>

## F.A.Q.:

> How long does it take a disk to fill?

When I tested the application on my systems, it filled about 1GB per second.
In my case a 1TB `C:\` disk would take ~16 minutes until it is "full".

:information_source: Performance might vary throughout different systems

<br>

> Why does it check for 4GB of free space?

It is not necessary, but I thought to myself, to leave some space available for the operating system itself.
I did not really test what would happen at a complete full `C:\` disk, but maybe this will be changed in the future.

<br>

>  How can I fill a disk much faster?

This could be achieved by starting multiple threads or increasing the size of the file content.

<br>

> Why is this application blocked by anti-virus software?

That might be because the application has sort of a malicious behaviour:
- The application has no window (size 0x0)
- The application itself is not visible in the taskbar
- The application writes much data to multiple files
