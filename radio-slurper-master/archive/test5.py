from twilio.rest import Client


# Your Account Sid and Auth Token from twilio.com/console
# DANGER! This is insecure. See http://twil.io/secure
account_sid = 'ACae4b1ada67a146d0bbf1d3769d6973c1'
auth_token = '7e7e3019b960390b5dd6eca416aa6169'
client = Client(account_sid, auth_token)

message = client.messages \
                .create(
                     body="Test",
                     from_='+14257806282‬',
                     to='+172881'
                 )

print(message.sid)