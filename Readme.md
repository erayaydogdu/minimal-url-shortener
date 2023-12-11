### Minimal URL Shortener with .NET Minimal API, htmx, and DaisyUI

This is a fun and lightweight project that combines the power of .NET Minimal API, htmx, and DaisyUI to create a robust web application for shortening and managing URLs. This application is built with C#, LiteDB, Hashids, and leverages the minimal API capabilities for an optimal experience. The addition of htmx ensures lightweight and fast frontend interactions, while DaisyUI provides additional styling with Tailwind CSS.

[![dotnet-cicd](https://github.com/erayaydogdu/minimal-url-shortener/actions/workflows/dotnet.yaml/badge.svg)](https://github.com/erayaydogdu/minimal-url-shortener/actions/workflows/dotnet.yaml)

## Getting Started

Follow one of these steps to set up and run the URL shortener service on your local machine:

**Clone the Repository:**
   ```bash
   git clone https://github.com/erayaydogdu/minimal-url-shortener.git
   cd minimal-url-shortener
   ```

**or run Docker:**
   ```bash 
   docker run -p 5148:8080 --name mus -v data_mus:/app --user root erayaydogdu/minimal-url-shortener
   # http://localhost:5148/
   ```


## Features

1. **URL Shortening:**
   - Shorten long URLs into compact and shareable links.
   - Utilizes Hashids to generate unique and user-friendly hash-like ID Urls.

2. **URL Redirection:**
   - Easily redirect the original URL by entering the short link.

3. **Minimal API:**
   - This is not a REST API; It's more likely BFF approach.
   - Returns HTML contents for AJAX calls, providing a tailored and efficient frontend interaction.

4. **Frontend Interactions:**
   - Lightweight and fast interactions powered by htmx for an efficient user experience.

## Technology Stack

This project makes use of the following technologies:

- **.NET Minimal API:** Leveraging the minimal API capabilities for a streamlined and efficient backend.
- **LiteDB:** A simple and lightweight NoSQL database used for storing URLs.
- **Hashids:** An efficient library for generating unique and human-readable hash-like IDs.
- **htmx:** A JavaScript library that enhances web pages with minimal JavaScript for seamless interactions.
- **DaisyUI with Tailwind CSS:** Additional styling and UI components provided by DaisyUI for a polished and aesthetic interface.