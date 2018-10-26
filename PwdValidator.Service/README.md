# MCMSPasswordValidator

MCMSPasswordValidator is a standalone, multi-platform and hopefully cloud-capable password validation tool.

  - Validate the SHA-! hash of a string against the list of breached passwords
  - Notify you how many occurrences have been found of the password
  - Track the unsafe passwords requested against the store.

# API Endpoints 

  - ...

### Tech Stack

MCMSPasswordValidator uses the following external libraries to work properly.  Note these packages are all managed through NuGet:

* [Firebird] - Open-source database capable of running in embedded mode
* [Microsoft Extensions CommandLineUtils] - Command Line Utils done the right way...
* [NPOI] - NPOI Excel Library (used for reporting services)

### Installation

MCMSPasswordValidtor requires [.NET Core Runtime or SDK](https://www.microsoft.com/net/download/) v2.1 to run.

Install the dependencies and devDependencies and start the server.

```sh
$ cd dillinger
$ npm install -d
$ node app
```

For production environments...

```sh
$ npm install --production
$ NODE_ENV=production node app
```

### Plugins

### Development

Want to contribute? Great!

Dillinger uses Gulp + Webpack for fast developing.
Make a change in your file and instantanously see your updates!

Open your favorite Terminal and run these commands.

First Tab:
```sh
$ node app
```

Second Tab:
```sh
$ gulp watch
```

(optional) Third:
```sh
$ karma test
```
#### Building for source
For production release:
```sh
$ gulp build --prod
```
Generating pre-built zip archives for distribution:
```sh
$ gulp build dist --prod
```
### Docker
Dillinger is very easy to install and deploy in a Docker container.

By default, the Docker will expose port 8080, so change this within the Dockerfile if necessary. When ready, simply use the Dockerfile to build the image.

```sh
cd dillinger
docker build -t joemccann/dillinger:${package.json.version}
```
This will create the dillinger image and pull in the necessary dependencies. Be sure to swap out `${package.json.version}` with the actual version of Dillinger.

Once done, run the Docker image and map the port to whatever you wish on your host. In this example, we simply map port 8000 of the host to port 8080 of the Docker (or whatever port was exposed in the Dockerfile):

```sh
docker run -d -p 8000:8080 --restart="always" <youruser>/dillinger:${package.json.version}
```

Verify the deployment by navigating to your server address in your preferred browser.

```sh
127.0.0.1:8000
```

#### Kubernetes + Google Cloud

See [KUBERNETES.md](https://github.com/joemccann/dillinger/blob/master/KUBERNETES.md)


### Todos

 - Add more tests
 - Validate the performance

### References

 - https://blogs.msdn.microsoft.com/premier_developer/2018/04/26/setting-up-net-core-configuration-providers/
 - https://stackoverflow.com/questions/46561660/net-core-2-0-windows-service
 - https://gist.github.com/iamarcel/8047384bfbe9941e52817cf14a79dc34

License
----
MIT