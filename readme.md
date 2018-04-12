# MBills

### The loop

- I call a method by their API
- They call a webhook that I have set up (reverse-api).

This is why I need to verify that it is really them when they are sending the response to the webhook. The provider signs each request and then I have to verify the signature.

### Need to setup a webhook

test za webhook










## Old information (might still be useful)

### Todos:

- Finish the HTTP client to actually be able to create requests to the server.

### How to implement

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