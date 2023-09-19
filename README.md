# Mapingway API

Backend application for travel routes managing

## Deployment

### Local development environment:

You can start up using `docker compose up` and compose.yaml file for Docker from **./docker/** directory context.

### Prerequisites:

**✔ Installed Docker Desktop/Engine.**

**✔ HTTPS SSL certificate.**

To enable HTTPS on local machine you need to:

1. Generate random string password for your local certificate.
2. Create SSL certificate and trust it for local devs:
   ```
   dotnet dev-certs https --trust -ep $env:USERPROFILE\.aspnet\https\Mapingway.API.pfx -p {your generated password}
   ```
3. Update the **.env** file with newly generated password:
![certificate-local-ssl-password.png](.github/readmecontent/certificate-local-ssl-password.png)

**✔ DBMS (e.g. [DBeaver](https://dbeaver.io/download/)).**

After local deployment you can connect to containerised database using credentials from the **.env** file:
![localdb-connection.png](.github/readmecontent/localdb-connection.png)












[Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet#links)