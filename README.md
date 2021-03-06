# Hackathon Submission Entry form

## Team name
### INDO-ARABIAN SITECORIANS

## Category
(1) Enhancement to the Sitecore Admin (XP) for Content Editors and Marketers

## Description

  - Flexibility while working with Content for Multi-lingual sites.
  - While creating a new language version for any content item, all fields are set to emtpy or null, thus creating some inconvience for Content Authors to know the exact content an already created language version of the item has.

  - This tool provides a flexibility to create multiple language versions for an item and its sub-items with default content loaded with a single-click. 
  
  - For example: If there are some large no. of Article/News Content Items already defined in an English Language (en-US) and the company decides to create a Spanish Language (es-US) version, creating language version for each item would be a time-consuming job as well as Powershell solution would not provide default content or non-technical feasibility. 

  
## Video link
⟹ [Replace this Video link](#video-link)

## Pre-requisites and Dependencies

⟹ For a plain vanilla Sitecore Instance, one more Language should be added to test this tool.

## Installation instructions
⟹ Installation requires 2 simple steps:

> - Install an additional language version in a plain vanilla Sitecore instance.
> - Install the package - IndoArabianLanguageVersionCopy-1.0.zip present in folder 



### Configuration
⟹ Please make sure AddFeatureVersionFromLanguage.config is present inside \App_Config\Include\Feature folder


## Usage instructions
- Create some Sample items under the Home item with default Content that will be created for English (en) version
- To create another language version (e.g it-IT or Italian) change the language version of the Home Item.
(Since this is a new language version no fields are shown.)

By default, the Content Author can create another version of this single item through Version tab without any default content in the newly created language version.

Using this tool, the Content Author can create a new language version of the selected item and its sub-items with default content copied from already existing language.

- Select the item > navigate to Version tab > click on "Copy from" drop button.
- Select the Language version you want to copy the entire content from. Check Deep Copy if....
Check Include Subitems, if you want to create language version for all the subitems under the selected item.






## Comments
If you'd like to make additional comments that is important for your module entry.
