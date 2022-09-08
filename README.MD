# Dot Net 6 Client for Stability Diffusion GRPC

This is a .Net 6 client for the Stability Diffusion GRPC API, pointed to Stability.ai's endpoint at grpc.stability.ai:443.

For more information on the API, visit the official SDK:

https://github.com/Stability-AI/stability-sdk

https://github.com/Stability-AI/api-interfaces

## Features

A simple command line interface, similar to existing Python scripts.  Both -- and - are allowed for params

    dotnet run --project StabilityDiffusionConsole --prompt "dog" --apikey "YOURAPIKEY"

    dotnet run --project StabilityDiffusionConsole --h 384 --w 384 -apikey "YOURAPIKEY" -prompt "a golden cat on a table"

...or compile and run the EXE

    StabilityDiffusionConsole -h 384 -w 384 -apikey "YOURAPIKEY" -prompt "a golden cat on a table"

Example output:

    9/8/2022 6:07:14 PM: Generated request. Request id: 2fa39844-70c8-43ed-be0d-adf91e111100, prompt: 'a golden cat on a table', h:384, w:384
    9/8/2022 6:07:17 PM: Keepalive received
    9/8/2022 6:07:17 PM: writing image/png: 00000.png
![a golden cat on a table](/00000.png)


To see all parameters supported, run without any parameters.

    Description:
    Request args

    Usage:
    StabilityDiffusionConsole [options]

    Options:
    -h <h>                            Height of the image to be created in pixels. [default: 512]
    -w <w>                            Width of the image to be created in pixels. [default: 512]
    -prompt <prompt>                  Text prompt. [default: A very large rodent having a bite of cake, wearing metallic steampunk armor, by Dr Seuss, trending on artstation]
    -engine <engine>                  engine [default: stable-diffusion-v1-5]
    -ddim_steps, -steps <ddim_steps>  Steps [default: 30]
    -outdir <outdir>                  Output directory [default: ./]
    --version                         Show version information
    -?, -h, --help                    Show help and usage information

## Near term

* Complete common parameters
* img2img support

## Possible Future Updates

* GRPC service side for self hosting
* React UI
* tinygrad backend for inference