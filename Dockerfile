FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /lib/redux-fsharp
COPY . /lib/redux-fsharp
RUN dotnet build