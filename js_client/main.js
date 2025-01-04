const grpc = require('@grpc/grpc-js');
const protoLoader = require('@grpc/proto-loader');
const path = require('path');

// Load the proto file
const PROTO_PATH = path.join(__dirname, "..","protos","protos_v1",'room.proto'); 
const PROTO_PATH_MESSAGES = path.join(__dirname, "..","protos","protos_v1",'message.proto'); 
console.log(PROTO_PATH);
console.log(PROTO_PATH_MESSAGES);
const packageDefinition = protoLoader.loadSync(PROTO_PATH, {
  keepCase: true,
  longs: String,
  enums: String,
  defaults: true,
  oneofs: true,
});
const packageDefinitionMessages = protoLoader.loadSync(PROTO_PATH_MESSAGES, {
    keepCase: true,
    longs: String,
    enums: String,
    defaults: true,
    oneofs: true,
  });
const roomsProto = grpc.loadPackageDefinition(packageDefinition).rooms;
const messageProto =grpc.loadPackageDefinition(packageDefinitionMessages).messages;

const client = new roomsProto.RoomExchangeService(
  '0.0.0.0:5038', 
  grpc.credentials.createInsecure()
);

const request = {
  requestId: '1234', 
};

const messageClient  = new messageProto.MessageExchangeService('0.0.0.0:5038', 
    grpc.credentials.createInsecure());
  const createRoomReq = {
      name:"js room",
      hostUserId:"luka",
      description:"opanaa"
  }
//   messageClient.CreateRoom(createRoomReq,(e,r)=>{
//       console.log(r)
//   })

client.ListAvailableRooms(request, (error, response) => {
  if (error) {
    console.error('Error:', error);
    return;
  }
  console.log('Available Rooms:', response.rooms);
});


