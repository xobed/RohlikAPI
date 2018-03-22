from checks import AgentCheck
import urllib2
import dateutil.parser
from datetime import datetime
import pytz
import json


class RohlikAgeCheck(AgentCheck):
    def check(self, instance):
        response = urllib2.urlopen('https://rohlikapi.azurewebsites.net/api/GetAllProducts')
        response_content = response.read()

        deserialized = json.loads(response_content)
        time_string = deserialized['SyncTime']
        time = dateutil.parser.parse(time_string)
        timediff = datetime.now(pytz.utc) - time
        hours = int(timediff.total_seconds() / 3600)
        self.gauge('rohlikapi.productsage', hours)        


if __name__ == '__main__':
    check, instances = RohlikAgeCheck.from_yaml('/etc/dd-agent/conf.d/rohlikAgeCheck.yaml')
    for instance in instances:
        print "\nRunning the check"
        check.check(instance)
        print "\nCheck finished"
