import json


def cleanStr4SQL(s):
    return s.replace("'", "`").replace("\n", " ")


def parseBusinessData():
    # read the JSON file
    # Assumes that the data files are available in the current directory. If not, you should set the path for the yelp data files.
    with open('yelp_business.JSON', 'r') as f:
        outfile = open('business.txt', 'w')
        line = f.readline()
        count_line = 0
        # read each JSON abject and extract data
        while line:
            data = json.loads(line)
            # business id
            outfile.write(cleanStr4SQL(data['business_id'])+'\t')
            # name
            outfile.write(cleanStr4SQL(data['name'])+'\t')
            # full_address
            outfile.write(cleanStr4SQL(data['address'])+'\t')
            # state
            outfile.write(cleanStr4SQL(data['state'])+'\t')
            # city
            outfile.write(cleanStr4SQL(data['city'])+'\t')
            # zipcode
            outfile.write(cleanStr4SQL(data['postal_code']) + '\t')
            # latitude
            outfile.write(str(data['latitude'])+'\t')
            # longitude
            outfile.write(str(data['longitude'])+'\t')
            # stars
            outfile.write(str(data['stars'])+'\t')
            # reviewcount
            outfile.write(str(data['review_count'])+'\t')
            # openstatus
            outfile.write(str(data['is_open'])+'\t')

            # category list
            categories = data["categories"].split(', ')
            outfile.write(str(categories)+'\t')
            outfile.write(str([]))  # write your own code to process attributes
            # write your own code to process hours
            outfile.write(str(data['hours']))
            outfile.write('\n')

            line = f.readline()
            count_line += 1
    print(count_line)
    outfile.close()
    f.close()


def parseUserData():
    # write code to parse yelp_user.JSON
    with open('yelp_user.JSON', 'r') as f:
        outfile = open('user.txt', 'w')
        line = f.readline()
        count_line = 0
        # read each JSON abject and extract data
        outfile.write('name' + '\t' + 'user_id' + '\t' + 'yelping_since' + '\t' + 'tipcount' + '\t' +
                      'funny' + '\t' + 'useful' + '\t' + 'cool' + '\t' + 'fans' + '\t' + 'average_stars' +
                      '\t' + 'friends' + '\n')
        while line:
            data = json.loads(line)
            # username
            outfile.write(cleanStr4SQL(data['name'])+'\t')
            # user ID
            outfile.write(cleanStr4SQL(data['user_id'])+'\t')
            # yelping start date
            outfile.write(cleanStr4SQL(data['yelping_since'])+'\t')
            # number of tips
            outfile.write(str(data['tipcount'])+'\t')
            # how many poeple found a tip funny
            outfile.write(str(data['funny'])+'\t')
            # how many pople found a tip useful
            outfile.write(str(data['useful']) + '\t')
            # how many people found a tip cool
            outfile.write(str(data['cool']) + '\t')
            # how many fans they have
            outfile.write(str(data['fans']) + '\t')
            # avg stars given
            outfile.write(str(data['average_stars']) + '\t')
            # the friends ofa user
            outfile.write(str(data['friends'])+'\t')
            outfile.write('\n')

            line = f.readline()
            count_line += 1
    print(count_line)
    outfile.close()
    f.close()


def parseCheckinData():
    # write code to parse yelp_checkin.JSON
    # write code to parse yelp_user.JSON
    with open('yelp_checkin.JSON', 'r') as f:
        outfile = open('checkin.txt', 'w')
        line = f.readline()
        count_line = 0
        # read each JSON abject and extract data
        outfile.write('business_id' + '\t' + 'check-ins' + '\n')
        while line:
            data = json.loads(line)
            # business_id
            outfile.write(cleanStr4SQL(data['business_id'])+'\t')
            # All chckins at this bussiness ID
            outfile.write(str(data['date'])+'\t')
            outfile.write('\n')

            line = f.readline()
            count_line += 1
    print(count_line)
    outfile.close()
    f.close()


def parseTipData():
    # write code to parse yelp_tip.JSON
    with open('yelp_tip.JSON', 'r') as f:
        outfile = open('tip.txt', 'w')
        line = f.readline()
        count_line = 0
        # read each JSON abject and extract data
        outfile.write('business_id' + '\t' + 'date' +
                      '\t' + 'likes' + '\t' + 'text' + '\n')
        while line:
            data = json.loads(line)
            # business_id
            outfile.write(cleanStr4SQL(data['business_id'])+'\t')
            # date
            outfile.write(cleanStr4SQL(data['date'])+'\t')
            # likes
            outfile.write(str(data['likes'])+'\t')
            # text
            outfile.write(str(data['text'])+'\t')
            outfile.write('\n')

            line = f.readline()
            count_line += 1
    print(count_line)
    outfile.close()
    f.close()


parseBusinessData()
parseUserData()
parseCheckinData()
parseTipData()
