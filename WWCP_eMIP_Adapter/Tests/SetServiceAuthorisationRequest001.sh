curl -v -X POST -H "Content-Type:   application/soap+xml" \
                -H "Accept:         application/soap+xml" \
				-H "Authorization:  0815" \
				-d @SetServiceAuthorisationRequest001.xml \
				http://127.0.0.1:3004/RNs/Prod/IO/Gireve
				