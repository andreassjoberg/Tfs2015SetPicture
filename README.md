# TFS 2015 Set Picture

Sets profile picture on a TFS2015 instance.

## Usage

From commandline, call the program with arguments like:  

	Tfs2015SetPicture.exe --servername <servername_or_ip> --username <your_user_name> --imagepath <path_to_your_image>
	
If <servername_or_ip> is a servername or ip without "http://" specified, it will be surrounded by: "http://{servername_or_ip}:8080/tfs".
To avoid this automatic surrounding, simply include "http://" in your <servername_or_ip>.
	
        private static Uri GetServerUri(string serverName)
        {
            if (serverName.Contains("http://"))
                return new Uri(serverName);
            return new Uri(string.Format("http://{0}:8080/tfs/", serverName));
        }
		