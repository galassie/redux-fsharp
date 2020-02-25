FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /lib/redux-fsharp
COPY . /lib/redux-fsharp
RUN dotnet build