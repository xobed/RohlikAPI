from checks import AgentCheck
import urllib2

class RohlikAgeCheck(AgentCheck):
    def check(self, instance):
        response = urllib2.urlopen('https://rohlikapi.azurewebsites.net/api/GetAllProductsAge')
        response_content = response.read()
        hours = int(response_content)
        self.gauge('rohlikapi.productsage', hours)

if __name__ == '__main__':
    check, instances = RohlikAgeCheck.from_yaml('/etc/dd-agent/conf.d/rohlikAgeCheck.yaml')
    for instance in instances:
        print "\nRunning the check"
        check.check(instance)
        print "\nCheck finished"
