# MBills

## Todos:

#### phase 1 (mock environment)
- Finish the HTTP client to be able to call their API
	- Implement the authorization as prescribed **OK**
	- Create an API class, which will implement the method calls to their API **IN PROGRESS**
- Try the rest of the calls to the fake API.
- **Implement the Point-of-sale quick payment transaction flow**
- Test this transaction flow on their mock service.
- Learn how to subscribe to the WebHooks that are triggered by their API calls.
- Verify whether also the webhooks are testable on their mock services.

#### phase 2 (production environment)
- Implement, verify and test all of the features that you have tested in the mock environment in their actual production environment.
- Ask them for the actual apiKey, secretKey and production api path. Also ask about the consequences of tampering with production. Are there any test servers?

#### phase 3
- **Learn how to verify whether their response is correct by verifying their API key.**
- This will involve their production public key, which they will deliver when we sign the contract.

## Vprasanja

#### Testno okolje

- Na testnem serverju bo pravilen odgovor vrnjen za vsak api-key ali secret-key. 
- Zato ne morem testirat, ce pravilno izracunam credentiale.
- Pod 'reference' so opisi vsake metode. In pod specifikacijo requesta v teh metodah sta navedena tudi primer secretkey, username in password, ceprav metode delajo s poljubnimi temi parameteri. Sem morda spregledal kaj, ali so te sampli tam samo zato ta bralec vidi v kaksnem formatu so naceloma te parametri?

- Imate kaksen testni streznik oz neko okolje, kjer lahko testiram brez posledic ce kaj zafrknem? Lahko to tudi v produkciji? Prosim za apiPath, apiKey in secretKey.

#### Webhooks

- Omenite, da se vecina status updejtov poslje preko webhookov, ki so en URL konfiguriran za tvoj API key. 
- V mock strezniku so vsi odgovori instantni, torej verjetno tega tukaj ni?
- Lahko to stestiram samo v produkciji?
- Kako pa se potem povezem na webhooke?

#### Verifikacija responsa

- Pravite da signate response in webhooke z SHA256withRSA (OID: 1.2.840.113549.1.1.11).
- To me pripelje do RFC 4055, kjer sta (nagnusno) predstavljena RSASSA-PSS, RSAES-OAEP.
- So detajli tega kar je treba narest v RFC 4055.

- Je to morda precej enostavno? @resitev
	- Pogledam njihov podpis
	- pri meni: hash = SHA1(apikey, nonce, timestamp + tid)
	- RSA^-1(podpis)
	- hash =? RSA^-1(podpis)
	- Slisi se simple in elegantno, ampak ne pozabi da nimam nonce, timestamp in tid!!!

- Ali se za produkcijo od vas dobi prav nek certifikat, ali je le javni kljuc?



## Open questions

- Problem: I can use any apiKey and secretKey and the test url will return that the call was correct?
	- It seems that the calls are just a mock api - whatever the apiKey and secret will be, the result will be the same.

- Can I recieve test apiKey, secretKey to have my way around the application?
	- Also I will need the real api root path!
	- Will the api calls have any real consequences?

- Most of the payment status updates are sent via webhooks which is a single URL configured for your API key. Where can I get this URL? Are there standard libraries for languages that can be used to connect to the webhook?

- Decorator pattern comes in handy every once in a while - check it out in free time.

## How to implement

#### How to authenticate to mBills server.

You need to receive ```apikey``` and ```secretkey```. ```secretkey``` is very valuable and should be kept very private.
 
- You call the API using HTTP Basic Authentication headers in the following way:
	- username: apikey.nonce.timestamp
	- password: hash(username + secretKey + request URL)
		- ```requestUrl``` = full request enpoint with any possible query parameters.
		- ```nonce``` - a random generated number between 8 and 15 digits.
		- ```timestamp``` - Timestamp for the request in epoch format GMT. Your servers need the correct time for the calls to work. A little time difference is tolerated, but not much.
		- ```hash``` - hex encoded SHA256 hash of username, secretKey, requestURL with no delimiter in concat.

#### How to verify mBills responses

- Also in the documentation.
		
## Documentation

### Verification of responses



## Their explainations

## Payments in trgovniske dejavnosti

### Summary and resources

- appropriate for 99% of customers
- How it goes:
	- **$user** has **$mbills** application.
	- **$user** shows a ```bar code``` within **$mbills**
	- **$cashier** reads ```bar code``` with a barcode reader
	- payment is processed through birokrat as a function of the read ```bar code```.
	- **$mbills_servers** confirms the transaction
	- **$cashier** fiscalizes and prints the receipt
- technical specification: https://mbillsquickpaymentapi.docs.apiary.io/#
- example of payment in video: ```https://www.youtube.com/watch?v=t7burEPvOAA```


### Detailed features

##### transaction
- merchant scans the barcode from the mbills system
- sends a sale request to the mBills system, which then confirms it
- **2 options are possible**:
	- **Quick pay**, where his confirmation is not needed
	- **No Quick pay**, where his confirmation is needed
		- The merchant makes a sale request using the user's barcode
		- The user is notified, and can view the details of the receipt on his device
		- The user confirms or discards the payment request
		- The merchant is notified about the user's decision via a webhook into his POS application

##### Loyalty support

- Can link mBills user to metchant's loyalty program

##### Preauthorizations, captures, voids

- The user can just reserve their payments. If something goes wrong with the payment, we may cancel the reservation.

##### Refunds

- Cash register will call the refund function on mBills.

##### Attaching bill specification

- Required before calling POS quick payment API. Specification is visualized in mBills. EOR can be sent after fiscallization.

##### WebHooks

- Most payment status updates are sent via webhooks which is a single URL configured for your API key.
- To try to simplify use of the system, all payment responses have the same body.
	- sale
	- invoice
	- status of transaction
	- webhooks
	- ...all kept the same

##### Loyalty membership

- linking loyalty between application and loyalty provider. Why?
	- to enable user to view loyalty membership data
	- to enable merchant's register to get loyalty mem data from user's payment token id
	- **to enable user to benefit from loyalty program without having to carry physical card**

There is a diagram on how to do this in the documentation!

### Open questions



- How long should we wait for the user to confirm the transaction after timing out?

## Payments in restaurants, bars (Will not support)

### Summary and resources

- appopriate for a small number of customers
- How it goes:
	- **$cashier** prints a fiscalized receipt with a FURS QR code ```QR```.
	- **$user** scans ```QR``` on the printed receipt and confirms the purchase.
	- **$cash_registry** listens to the event that the payment has been successful.
- example of payment in video: ```https://www.youtube.com/watch?v=RqQkKWo0au0```

- technical specification: ```https://mbillsonlinepaymentsapi.docs.apiary.io/#```
