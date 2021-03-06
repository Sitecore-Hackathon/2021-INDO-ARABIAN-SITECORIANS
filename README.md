# Hackathon Submission Entry form

## Team name
### INDO-ARABIAN SITECORIANS

## Category
Enhancement to the Sitecore Admin (XP) for Content Editors and Marketers

(1) Quick Version Creator:
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

(2) AddThis Plugin for Sitecore:
## Description

  - The use vase of sharing the pages on website on Social Media channels is very common. 
  - Most of the teams create it manually s a .Net component or some teams by paid thirdparty plugins to give the authors/marketers the capability to share the content on Social Media.

  - This tool provides the expected functionality but also powers the marketer's expectation by giving the analytics of which all URLs are shared, from which device it is shared, etc. 
  
  - For example: If there are some large no. of users from India sharing the content from your site, then India can be your hot market to target for in all the campaigns. 

  
## Video link
⟹ [Replace this Video link](#video-link)

## Pre-requisites and Dependencies

⟹ For a plain vanilla Sitecore Instance, install the AddThis Plugin for Sitecore-1.0.zip and you are good to go.

## Installation instructions
⟹ Installation requires 2 simple steps:

> - Install the AddThis Plugin for Sitecore-1.0.zip.
> - Add the rendering "AddThis Plugin Social Share" under the path "/sitecore/layout/Renderings/Feature/Social Share/AddThis Plugin Social Share" on any page where you want the Social Share options with the placeholder (Ex. main) 

### Configuration
⟹ Please make sure you create the AddThis account and add the id or use the one which present for Demo purpose here - /sitecore/system/Modules/AddThis Plugin/AddThis Dashboard Details.

## Usage instructions
- Track all the activities on the Social Media sharing options here - https://www.addthis.com/dashboard#dashboard-analytics

## Comments
If you'd like to make additional comments that is important for your module entry.
