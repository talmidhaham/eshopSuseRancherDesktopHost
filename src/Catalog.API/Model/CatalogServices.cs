﻿using eShop.Catalog.API.Services;

public class CatalogServices(
    CatalogContext context,
    ICatalogAI catalogAI,
    IOptions<CatalogOptions> options,
    ILogger<CatalogServices> logger,
    ICatalogIntegrationEventService eventService)
    //IConfiguration configuration )
{
    public CatalogContext Context { get; } = context;
    public ICatalogAI CatalogAI { get; } = catalogAI;
    public IOptions<CatalogOptions> Options { get; } = options;
    public ILogger<CatalogServices> Logger { get; } = logger;

    //public IConfiguration Conf { get; } = configuration;

    public ICatalogIntegrationEventService EventService { get; } = eventService;
};
