// TODO shenaxva dzebnis optimizacia
#ifdef __linux__ // file server marto linuxze
#include<iostream>
#include<fstream>
#include<cstring>
#include<arpa/inet.h>
#include<thread>
#include<unistd.h>

inline void LOG_INFO(const char* msg)
{
    std::cout << "Info: " << msg << std::endl;
}
inline void LOG_ERROR(const char* msg)
{
    std::cerr << "Error: " << msg << std::endl;
}

constexpr int PORT = 8080;
constexpr int BUFFER_SIZE = 4096;

void handle_client(int clientSocket)
{
    char buffer[BUFFER_SIZE];
    ssize_t bytesRead;

    bytesRead = recv(clientSocket, buffer, sizeof(uint32_t), 0);
    if (bytesRead <= 0)
    {
        LOG_ERROR("Failed to read metadata");
        close(clientSocket);
        return;
    }

    uint32_t fileNameLength;
    std::memcpy(&fileNameLength, buffer, sizeof(uint32_t));
    fileNameLength = ntohl(fileNameLength);

    std::cout << fileNameLength << std::endl;

    if (fileNameLength == 0 || fileNameLength > BUFFER_SIZE - sizeof(uint32_t))
    {
        LOG_ERROR("Invalid file name length");
        close(clientSocket);
        return;
    }

    bytesRead = recv(clientSocket, buffer, fileNameLength, 0);
    if (bytesRead != fileNameLength)
    {
        LOG_ERROR("Failed to read file name");
        close(clientSocket);
        return;
    }

    std::string fileName(buffer, fileNameLength);
    std::ofstream file(fileName, std::ios::binary);
    if (!file.is_open())
    {
        LOG_ERROR("Error: unable to open file");
        close(clientSocket);
        return;
    }

    size_t totalBytes = 0;
    LOG_INFO(("Receiving file: " + fileName).c_str());
    while ((bytesRead = recv(clientSocket, buffer, BUFFER_SIZE, 0)) > 0)
    {
        totalBytes += bytesRead;
        file.write(buffer, bytesRead);
    }

    if (bytesRead == 0)
    {
        LOG_INFO("File transfer completed");
    }
    else {
        LOG_ERROR("File transfer failed");
    }

    file.close();
    close(clientSocket);
}

int main()
{
    int serverSocket = socket(AF_INET, SOCK_STREAM, 0);
    if (serverSocket == -1)
    {
        LOG_ERROR("Failed to create socket");
        return 1;
    }

    sockaddr_in serverAddr{};
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_addr.s_addr = INADDR_ANY;
    serverAddr.sin_port = htons(PORT);

    if (bind(serverSocket, (struct sockaddr*)&serverAddr, sizeof(serverAddr)) == -1)
    {
        LOG_ERROR("Failed to bind socket");
        close(serverSocket);
        return 1;
    }

    if (listen(serverSocket, 1) == -1)
    {
        LOG_ERROR("Failed to listen on socket");
        close(serverSocket);
        return 1;
    }

    LOG_INFO("Listening ...");
    while (true)
    {
        sockaddr_in clientAddr{};
        socklen_t clientAddrLen = sizeof(clientAddr);
        int clientSocket = accept(serverSocket, (struct sockaddr*)&clientAddr, &clientAddrLen);
        if (clientSocket == -1)
        {
            LOG_ERROR("Failed to accept connection");
            continue;
        }

        LOG_INFO("Client connected");
        std::thread clientThread(handle_client, clientSocket);
        clientThread.detach();
    }

    close(serverSocket);
    return 0;
}

#endif
