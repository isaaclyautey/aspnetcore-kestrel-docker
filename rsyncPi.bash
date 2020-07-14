rsync -avzh /home/isaac/Projects/PersonalSite/ isaac@alarmpi:/home/isaac/Projects/PersonalSite/
ssh isaac@alarmpi docker-compose -f '/home/isaac/Projects/PersonalSite/docker-compose.yml' up -d --build