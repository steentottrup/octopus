#r "../Newtonsoft.Json.dll";
using System.Net;
using System.Linq;
using System.Threading;

var suiteId = Octopus.Parameters["SuiteID"];
var apiKey = Octopus.Parameters["GhostInspectorAPIKey"];
var baseUrl = Octopus.Parameters["SiteBaseUrl"];

ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
WebClient client = new WebClient();

String startTestApiCall = $"https://api.ghostinspector.com/v1/suites/{suiteId}/execute/?immediate=1&apiKey={apiKey}&startUrl={baseUrl}";
Console.WriteLine($"Starting test with a call to {startTestApiCall}");
String runOutput = client.DownloadString(startTestApiCall);

RunOutput output1 = Newtonsoft.Json.JsonConvert.DeserializeObject<RunOutput>(runOutput);

Console.WriteLine($"Suite test has been started, looking for result for {output1.Data._id}");

Int32 maxWait = 120;
Int32 seconds = 10;
Int32 waited = 0;
Boolean done = false;
Boolean passing = false;

String statusApiCall = $"https://api.ghostinspector.com/v1/suites/{suiteId}/results/?apiKey={apiKey}";
Console.WriteLine($"Status calls will be made to {statusApiCall}");

while (done == false) {
    Console.WriteLine($"We'll wait for another {seconds} seconds, before asking for a result");
	Thread.Sleep(seconds * 1000);

	String statusOutput = client.DownloadString(statusApiCall);
	if (String.IsNullOrWhiteSpace(statusOutput) == true) {
      throw new ApplicationException("status request returned an empty string");
    }
  	Console.WriteLine("Got data back from the status request");
  	StatusOutput output2 = null;
  	try {
		output2 = Newtonsoft.Json.JsonConvert.DeserializeObject<StatusOutput>(statusOutput);
    }
  	catch (Exception e) {
      Console.WriteLine("Converting failed, " + e.StackTrace);
    }
  	Console.WriteLine("Let's look at the data");
	if (output2 != null && output2.Data != null && output2.Data.Any() == true) {
		done = output1.Data._id == output2.Data.First()._id;
		if (done == true && output2.Data.First().Passing.HasValue == true) {
            Console.WriteLine("The suite test is done");
			passing = output2.Data.First().Passing.Value;
		}
	}
  	else {
      Console.WriteLine("No data returned it seems");
      Console.WriteLine($"output was empty {output2 == null}");
      Console.WriteLine($"output was empty {output2.Data == null}");
      if (output2 != null) {
        Console.WriteLine($"Status {output2.Code}" + statusOutput);
      }
  	}
  	waited = waited + seconds;
  	if (waited > maxWait) {
      break;
    }
}

if (passing == true) {
  Console.WriteLine("Suite passed!");
}
else {
  throw new ApplicationException("Suite failed");
}



public class StatusOutput {
	public String Code { get; set; }
	public TestOutput[] Data { get; set; }
}

public class TestOutput {
	public String _id { get; set; }
	public Boolean? Passing { get; set; }
}

public class RunOutput {
	public String Code { get; set; }
	public RunOutputData Data { get; set; }
}

public class RunOutputData {
	public String _id { get; set; }
}
